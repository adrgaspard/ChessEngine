using ChessEngine.AI;
using ChessEngine.Core.Environment;
using ChessEngine.Core.Interactions.Contracts;
using ChessEngine.Core.Interactions.Generation;
using ChessEngine.Core.Interactions.Migration;
using ChessEngine.Core.Match;
using ChessEngine.Core.Serialization.Contracts;
using ChessEngine.Core.Serialization.FEN;
using ChessEngine.Core.Serialization.FEN.Tools;
using ChessEngine.Core.Transposition.Contracts;
using ChessEngine.Core.Transposition.Zobrist;
using ChessEngine.MVVM.Models;
using ChessEngine.MVVM.Utils;
using ChessEngine.MVVM.ViewModels.Abstractions;
using Microsoft.Toolkit.Mvvm.Input;
using System.ComponentModel;
using System.Windows.Input;

namespace ChessEngine.MVVM.ViewModels
{
    public class GameManagerViewModel : ViewModelBase
    {
        protected GameViewModel? gameVM;
        public GameViewModel? GameVM
        {
            get => gameVM;
            protected set
            {
                if (gameVM != value)
                {
                    if (value is not null)
                    {
                        value.PropertyChanged += OnGameVMPropertyChanged;
                    }
                    if (gameVM is not null)
                    {
                        gameVM.PropertyChanged -= OnGameVMPropertyChanged;
                    }
                    gameVM = value;
                    UpdateGameVMStateProperties();
                    RaisePropertyChanged();
                }
            }
        }

        protected ClockParameters clockParameters;
        public ClockParameters ClockParameters
        {
            get => clockParameters;
            set
            {
                if (clockParameters != value)
                {
                    clockParameters = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool CanStart => GameVM?.HasStarted is false;

        public bool CanInterrupt => GameVM?.HasStarted is true && GameVM?.EndGameType is EndGameType.GameIsNotFinished;

        public bool CanReset => !CanStart && !CanInterrupt;

        protected IDispatcherService DispatcherService { get; init; }

        protected AIFactory AIFactory { get; init; }

        protected IGameHashing<ulong> GameHashing { get; init; }

        protected IGameLoader<string> GameLoader { get; init; }

        protected IMovementGenerator GameMovementGenerator { get; init; }

        protected IMovementGenerator AIQuietMovementGenerator { get; init; }

        protected IMovementGenerator AIMovementGenerator { get; init; }

        protected IMovementMigrator GameMovementMigrator { get; init; }

        protected IMovementMigrator AIMovementMigrator { get; init; }

        protected IAttackDataGenerator AttackDataGenerator { get; init; }

        protected IEndGameChecker EndGameChecker { get; init; }

        public ICommand StartCommand { get; protected init; }

        public ICommand InterruptCommand { get; protected init; }

        public ICommand ResetCommand { get; protected init; }

        public GameManagerViewModel(IDispatcherService dispatcherService)
        {
            DispatcherService = dispatcherService;
            ClockParameters = ClockParametersConsts.Blitz3Plus0;
            GameHashing = new ZobristHashing();
            GameLoader = new FENGameLoader(GameHashing);
            AttackDataGenerator = new AttackDataGenerator();
            GameMovementGenerator = new MovementGenerator(PromotionGenerationType.AllPromotions, true);
            AIQuietMovementGenerator = new MovementGenerator(PromotionGenerationType.PromotionToQueenAndKnightOnly, false);
            AIMovementGenerator = new MovementGenerator(PromotionGenerationType.PromotionToQueenAndKnightOnly, true);
            GameMovementMigrator = new MovementMigrator(GameHashing, true);
            AIMovementMigrator = new MovementMigrator(GameHashing, false);
            EndGameChecker = new EndGameChecker();
            AIFactory = new(AttackDataGenerator, AIQuietMovementGenerator, AIMovementGenerator, AIMovementMigrator);
            StartCommand = new RelayCommand(Start);
            InterruptCommand = new RelayCommand(Interrupt);
            ResetCommand = new RelayCommand(Reset);
            GameVM = GenerateNewGameViewModel();
        }

        protected void Start()
        {
            if (!CanStart)
            {
                throw new InvalidOperationException($"The {nameof(GameViewModel)} can't be started.");
            }
            GameVM?.StartCommand.Execute(null);
        }

        protected void Interrupt()
        {
            if (!CanInterrupt)
            {
                throw new InvalidOperationException($"The {nameof(GameViewModel)} can't be interrupted.");
            }
            GameVM?.InterruptCommand.Execute(null);
        }

        protected void Reset()
        {
            if (!CanReset)
            {
                throw new InvalidOperationException($"The {nameof(GameViewModel)} can't be reset.");
            }
            GameVM = GenerateNewGameViewModel();
        }

        protected Func<GameViewModel, IDictionary<Colour, PlayerViewModel>> GetPlayersFactory()
        {
            return (gameVM) => new Dictionary<Colour, PlayerViewModel>(3)
            {
                { Colour.White, new LocalHumanPlayerViewModel(gameVM, DispatcherService, Colour.White, false) },
                { Colour.Black, new AIPlayerViewModel(gameVM, DispatcherService, AIFactory.GetAI_Version_5()) },
                { Colour.White | Colour.Black, new LocalHumanPlayerViewModel(gameVM, DispatcherService, Colour.White, true) },
            };
        }

        protected Func<IDictionary<Colour, ClockViewModel>> GetClocksFactory()
        {
            return () => new Dictionary<Colour, ClockViewModel>(2)
            {
                { Colour.White, new(ClockParameters) },
                { Colour.Black, new(ClockParameters) }
            };
        }

        protected virtual GameViewModel GenerateNewGameViewModel()
        {
            return new(GameLoader.Load(FENConsts.StartFEN), AttackDataGenerator, GameMovementGenerator, GameMovementMigrator, GameHashing, EndGameChecker, GetPlayersFactory(), GetClocksFactory());
        }

        protected void OnGameVMPropertyChanged(object? sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == nameof(GameViewModel.HasStarted) || eventArgs.PropertyName == nameof(GameViewModel.EndGameType))
            {
                UpdateGameVMStateProperties();
            }
        }

        protected void UpdateGameVMStateProperties()
        {
            RaisePropertyChanged(nameof(CanStart));
            RaisePropertyChanged(nameof(CanInterrupt));
            RaisePropertyChanged(nameof(CanReset));
        }
    }
}
