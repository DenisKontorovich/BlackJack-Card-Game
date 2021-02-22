.�n��*���;�YU(�[)-�� ��+Er����j�4EY\n���12���.����tA��Wh5\"4�=���ՙ�4��A��Yy)J�}�8�[��1��:�K��S(�?�%ZM�TS�
��FP��1z#��
�'j�e�1m�y
?B�Ĩ���Ҙ�C�$�C���U�6��>�te�ľQ3�8�����{��q�HX�)ɮi"<W�e/��N�yTR��T��0 U�%���
�4WLZ�� V�¾���% �N��q+�C�M��Ut�:���H��@�S�ğ����y�$-i���Jɏ�����;�U�vV����tΌ���1���YKl�������x� �o"
w4�s4g�i"��4��jS�C���a�,硱���|�����rṔ},��;G��K�h���J݇��'���W+���3�J��㲉�	.7�'b0�E",��O&�`hz�1��J��k��s֬8v>>��,�Z���:�TlX�v��C_��AP��he player hits and their total goes over 21 then they bust. The player can only 
 initially see the dealers first card, the dealer gets an automated turn where the dealer continuosly hits until 
 their total is equal or greater than 17. At this point the game is considered over and the winner is calculated.
 The user starts with a certain amount of money and can place bets on the game if they have enough in their wallet.
 Note: If a card has a number on it then it's value is the number on the card, however if the card is a jack, king, or queen
 the value of the card is 10. For aces the value can either be 1 or 11, if the card being valued at 11 will cause either
 the hand to go over 21 then the value of the ace will be 1. */
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Animation2D;

namespace BlackJack_Start
{
    public class BlackJack : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //Store both the current and previous mouse states for user input
        static MouseState mouse;
        static MouseState prevMouse;

        //The random object allowing for random number generation during Shuffle
        Random rng = new Random();

        //Store all possible collecions of Cards in the game
        Card[] deck = new Card[52];
        Card[] pHand = new Card[12];
        Card[] dHand = new Card[12];

        //The various timers used throughout the game
        float dealCardsTimer;
        float dealerTurnTimer;
        float exitGameTimer;

        //Store the images for the game
        Texture2D deckImg;
        Texture2D faceDownImg;
        Texture2D pregameBgImg;
        Texture2D postgameBgImg;
        Texture2D bettingBgImg;
        Texture2D betButtonImg;
        Texture2D exitButtonImg;
        Texture2D hitButtonImg;
        Texture2D standButtonImg;
        Texture2D resetButtonImg;
        Texture2D nextButtonImg;
        Texture2D doubleDownButtonImg;
        Texture2D gameplayBgImg;
        Texture2D whiteBoxImg;
        Texture2D whitePageImg;
        Texture2D instructionsBgImg;
        Texture2D instructionsButtonImg;
        Texture2D backButtonImg;

        //Rectangle variables for images
        Rectangle bgRec;
        Rectangle deckRec;
        Rectangle betRec;
        Rectangle bettingBetRec;
        Rectangle exitRec;
        Rectangle hitRec;
        Rectangle standRec;
        Rectangle resetRec;
        Rectangle nextRec;
        Rectangle doubleDownRec;
        Rectangle[] whiteBoxRec = new Rectangle[10];
        Rectangle whitePageRec;
        Rectangle instructionsBgRec;
        Rectangle instructionsButtonRec;
        Rectangle backButtonRec;

        //Set locations for texts displayed
        Vector2 walletLoc = new Vector2(15, 415);
        Vector2 betLoc = new Vector2(303, 305);
        Vector2 currentBetLoc = new Vector2(85, 305);
        Vector2 endingStatementLoc = new Vector2(220, 150);
        Vector2 endingStatementLoc2 = new Vector2(120, 210);
        Vector2 betAmountLabelLoc = new Vector2(500, 200);
        Vector2 walletLabelLoc = new Vector2(500, 240);
        Vector2 dealerTotalLabelLoc = new Vector2(30, 130);
        Vector2 playerTotalLabelLoc = new Vector2(30, 295);
        Vector2[] numbersLoc = new Vector2[10];
        Vector2 dealerLabelPostGameLoc = new Vector2(610, 95);
        Vector2 playerLabelPostGameLoc = new Vector2(610, 140);
        Vector2 resultLabelLoc = new Vector2(240, 95);
        Vector2 walletPostGameLoc = new Vector2(225, 250);
        Vector2 betLabelPostGameLoc = new Vector2(225, 310);
        Vector2 betPostGameLoc = new Vector2(335, 310);
        Vector2 outcomeLabelLoc = new Vector2(140, 180);
        Vector2 numCardsLoc = new Vector2(450, 10);

        //Store the fonts used throughout the game
        SpriteFont labelFont;
        SpriteFont numberFont;
        SpriteFont resultsFont;

        //Array of strings to store the numbers the user can chose 
        //when entering the amount they want to bet
        string[] numbers = new string[10];

        //String variables used to output various text in Game
        string walletLabel = "Wallet: $";
        string dollarSign = "$";
        string betAmountLabel = "Bet Amount: $";
        string betAmountOutput = "";
        string currentBetOutput = "Current Bet: ";
        string endingStatement = "Thank you for playing!";
        string endingStatement2 = "You are leaving the game with $";
        string dealerTotalLabel = "Dealer Total: ";
        string playerTotalLabel = "Player Total: ";
        string dealerLabel = "Dealer: ";
        string playerLabel = "Player: ";
        string resultsLabel = "Results";
        string plusSign = "+";
        string minusSign = "-";
        string dBlackJackOutput = "Dealer won with Blackjack";
        string pBlackJackOutput = "Player won with Blackjack";
        string dWinPointsOutput = "Dealer won by Points";
        string pWinPointsOutput = "Player won by Points";
        string dBustOutput = "Player won by dealer Bust";
        string pBustOutput = "Dealer won by player Bust";
        string pushOutput = "Game ended with a Push";
        string betPostGameLabel = "Bet $: ";

        //Maintain all the possible game states
        int gameState = PREGAME;
        const int PREGAME = 0;
        const int INSTRUCTIONS = 1;
        const int BETTING = 2;
        const int DEAL_CARDS = 3;
        const int PLAYER_TURN = 4;
        const int DEALER_TURN = 5;
        const int POST_GAME = 6;
        const int END_GAME = 7;

        //variables representing number of cards in deck
        //and maximum cards a player/dealer can have
        const int CARDS = 52;
        const int MAX_CARDS = 12;

        //initial variables for the x and y coordinates of various images
        const int WB_X_INITIAL = 60;
        const int X_INITIAL = 30;
        const int PY_INITIAL = 350;
        const int DY_INITIAL = 10;
        const int SPACER = 20;

        //Store the dimensions of the screen and the cards
        int screenWidth = 800;
        int screenHeight = 480;
        int cardWidth = 0;
        int cardHeight = 0;

        //Random numbers used to shuffle deck
        int randomNum;
        int randomNum2;

        //Store a number inidcating where the current top of the deck is
        int topOfDeck = 0;
        
        //Store the number of cards and their total 
        //in the dealer's and player's hands
        int numPCards = 0;
        int numDCards = 0;
        int pTotal = 0;
        int dTotal = 0;

        //Store the amount of money the user has, initially at $200
        //Store the amount of money user wants to bet
        int wallet = 200;
        int betAmount = 0;

        //Store the amount of cards dealt to the dealer and the
        //amount of cards left in the deck
        int cardCount = 0;
        int cardsInDeck = 52;

        //Bool array for whether user is hovering over
        //a certain box when making a bet
        bool[] boxHover = new bool[9];

        //Bool variable for whether it is the player's turn to recieve a card
        bool pTurn = true;

        //Bool variable for whether the bet is valid
        bool isBetValid = false;

        //Bool array for the results of the game
        bool[] gameResults = new bool[7];

        //Bool variable for whether the player has won or not
        bool playerWin;

        //Bool variable for whether to calculate the new 
        //Value of the user's wallet
        bool calcWallet = true;

        //Bool variable for whether game is to be reset in pregame
        bool resetGame = true;

        //Bool variable for whether double down button is shown
        bool doubleDownShow;

        //Sound effects
        SoundEffect dealingCards;
        SoundEffect buttonClick;
        SoundEffect gameWon;
        SoundEffect gameLost;

        public BlackJack()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            //Allow cursor to be seen by the user in game
            this.IsMouseVisible = true;

            base.Initialize();
        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load all card images
            deckImg = Content.Load<Texture2D>("images/Sprites/CardFaces");
            faceDownImg = Content.Load<Texture2D>("images/Sprites/CardBack");

            //Load  all background images
            pregameBgImg = Content.Load<Texture2D>("images/Background/PregameBg");
            bettingBgImg = Content.Load<Texture2D>("images/Background/BetWindowBg");
            gameplayBgImg = Content.Load<Texture2D>("images/Background/table");
            postgameBgImg = Content.Load<Texture2D>("images/Background/PostgameBg");
            instructionsBgImg = Content.Load<Texture2D>("images/Background/InstructionsBg");

            //Load all button images
            betButtonImg = Content.Load<Texture2D>("images/Sprites/BetButton");
            hitButtonImg = Content.Load<Texture2D>("images/Sprites/HitButton");
            standButtonImg = Content.Load<Texture2D>("images/Sprites/StandButton");
            doubleDownButtonImg = Content.Load<Texture2D>("images/Sprites/DoubleDownButton");
            exitButtonImg = Content.Load<Texture2D>("images/Sprites/ExitButton");
            resetButtonImg = Content.Load<Texture2D>("images/Sprites/ResetButton");
            nextButtonImg = Content.Load<Texture2D>("images/Sprites/NextButton");
            instructionsButtonImg = Content.Load<Texture2D>("images/Sprites/InstructionsButton");
            backButtonImg = Content.Load<Texture2D>("images/Sprites/BackButton");

            //Load the white box image used in the betting game state
            whiteBoxImg = Content.Load<Texture2D>("images/Background/WhiteBox");

            //Load white page image used in the post game game state
            whitePageImg = Content.Load<Texture2D>("images/Background/BlankPage");

            //Load all fonts
            labelFont = Content.Load<SpriteFont>("Fonts/LabelFont");
            numberFont = Content.Load<SpriteFont>("Fonts/NumberFont");
            resultsFont = Content.Load<SpriteFont>("Fonts/ResultsFont");

            //Load the dimensions of the cards
            cardWidth = deckImg.Width / Card.CARDS_IN_SUIT;
            cardHeight = deckImg.Height / Card.NUM_SUITS;

            //Load all rectangles
            bgRec = new Rectangle(0, 0, screenWidth, screenHeight);
            deckRec = new Rectangle(screenWidth - 150, 40, cardWidth, cardHeight);
            betRec = new Rectangle(210, 220, betButtonImg.Width / 2, betButtonImg.Height / 2);
            doubleDownRec = new Rectangle(10, 180, (int)(doubleDownButtonImg.Width / 2.5), (int)(doubleDownButtonImg.Height / 2.5));
            hitRec = new Rectangle(250, 180, (int)(hitButtonImg.Width / 2.5), (int)(hitButtonImg.Height / 2.5));
            standRec = new Rectangle(375, 180, (int)(standButtonImg.Width / 2.5), (int)(standButtonImg.Height / 2.5));
            bettingBetRec = new Rectangle(680, 350, (int)(betButtonImg.Width / 2.5), (int)(betButtonImg.Height / 2.5));
            resetRec = new Rectangle(550, 350, (int)(resetButtonImg.Width / 2.5), (int)(resetButtonImg.Height / 2.5));
            exitRec = new Rectangle(465, 220, exitButtonImg.Width / 2, exitButtonImg.Height / 2);
            nextRec = new Rectangle(620, 240, exitButtonImg.Width / 2, exitButtonImg.Height / 2);
            whitePageRec = new Rectangle(0, 90, screenWidth, (int)(screenWidth / 2.5));
            instructionsButtonRec = new Rectangle(660, 340, (int)(instructionsButtonImg.Width / 2.5), (int)(instructionsButtonImg.Height / 2.5));
            backButtonRec = new Rectangle(0, 0, (int)(backButtonImg.Width / 2.5), (int)(backButtonImg.Height / 2.5));
            instructionsBgRec = new Rectangle(0, 0, screenWidth, screenHeight);


            //For all the numbers between 0 and whiteBoxRec.Length load the appropriate rectangle
            for (int i = 0; i < whiteBoxRec.Length; i++)
            {
                //Create rectangle and for every increase in the number i move the x coordinate over to the right by a certain amount
                whiteBoxRec[i] = new Rectangle(WB_X_INITIAL + (int)((whiteBoxImg.Width / 2.9) * i), 215, (int)(whiteBoxImg.Width / 2.9), (int)(whiteBoxImg.Height / 2.9));

                //Convert the number i into a string and store it under the the numbers array 
                string iOutput = Convert.ToString(i);
                numbers[i] = iOutput;

                //For every increase in the number i move the location of the text to the right by a certain amount
                numbersLoc[i] = new Vector2(80 + (whiteBoxRec[i].Width * i), 225);
            }
            
            //Create the initial deck
            CreateDeck();

            //Shuffle the deck before going into PREGAME, this will need to be done each time
            ShuffleDeck(1000);

            //Load sound effects
            dealingCards = Content.Load<SoundEffect>("Audio/SoundEffects/DealingCards");
            buttonClick = Content.Load<SoundEffect>("Audio/SoundEffects/buttonClick");
            gameWon = Content.Load<SoundEffect>("Audio/SoundEffects/LadderEffect");
            gameLost = Content.Load<SoundEffect>("Audio/SoundEffects/SnakeEffect");

            //Adjust the volume of the soundeffects
            MediaPlayer.Volume = 0.7f;    
        }

        protected override void UnloadContent()
        {            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Get the current mouse state
            prevMouse = mouse;
            mouse = Mouse.GetState();

            //Perform the appropriate task based on which screen the player is on
            switch (gameState)
            {
                case PREGAME:
                    {
                        //When it is time to reset the game perform the appropriate tasks
                        if (resetGame == true)
                        {
                            //Recreate the initial deck
                            CreateDeck();

                            //Reshuffle the deck
                            ShuffleDeck(1000);

                            //The game is no longer needed to be reset
                            resetGame = false;
                        }

                        //When the mouse is clicked  perform the appropriate tasks
                        if (NewMouseClick() == true)
                        {
                            //When the user has more than $0 track if bet button was clicked
                            if (wallet > 0)
                            {
                                //When the mouse location of the user is in the betting button
                                if (MouseClicked(betRec) == true)
                                {
                                    //Play buttonClick sound effect
                                    buttonClick.CreateInstance().Play();

                                    //The current game state is now in betting
                                    gameState = BETTING;
                                }
                            }
                            
                            //When the mouse location of the user in in the exit button
                            if (MouseClicked(exitRec) == true)
                            {
                                //Play buttonClick sound effect
                                buttonClick.CreateInstance().Play();

                                //The current game state is now in end game
                                gameState = END_GAME;
                            }
                            
                            //When the mouse location of the user in in the instructions button
                            if (MouseClicked(instructionsButtonRec) == true)
                            {
                                //Play buttonClick sound effect
                                buttonClick.CreateInstance().Play();

                                //The current game state is now in instructions
                                gameState = INSTRUCTIONS;
                            }
                        }
                    }
                    break;
                case INSTRUCTIONS:
                    {
                        //When the mouse location of the user in in the back button
                        if (NewMouseClick() && MouseClicked(backButtonRec) == true)
                        {
                            //Play buttonClick sound effect
                            buttonClick.CreateInstance().Play();

                            //The current game state is now in pregame
                            gameState = PREGAME;
                        }
                    }
                    break;
                case BETTING:
                    {
                       //For all the numbers between 0 and numbers.Length
                        for (int i = 0; i < numbers.Length; i++)
                        {
                            //When the user is hovering over a specific box indicated by the number i
                            //perform the appropriate tasks
                            if (BoxHover(i) == true)
                            {
                                
                                //When the bet entered by the user is less than 7 digits
                                //track if mouse is clicked
                                if (betAmountOutput.Length < 7)
                                {
                                    //If the mouse is clicked
                                    if (NewMouseClick() == true)
                                    {
                                        //Play button click sound effect
                                        buttonClick.CreateInstance().Play();

                                        //Call BoxClicked to get the bet amount
                                        BoxClicked(i);
                                    }
                                }
                            }
                        }

                        //When the bet amount is greater than amount in the user's wallet
                        // or is equal to $0
                        if (betAmount > wallet || betAmount == 0)
                        {
                            //bet is invalid
                            isBetValid = false;
                        }
                        //When the bet amount is less than the amount in the user's wallet
                        //and is greater than $0
                        else if (betAmount <= wallet || betAmount != 0)
                        {
                            //bet is valid
                            isBetValid = true;
                        }

                        //When the mouse is clicked perform the appropriate tasks
                        if (NewMouseClick() == true)
                        {
                            //When the bet entered is valid track whether bet button was clicked
                            if (isBetValid == true)
                            {
                                //When the mouse location is within the betting game state bet button
                                if (MouseClicked(bettingBetRec) == true)
                                {
                                    //Play button click sound effect
                                    buttonClick.CreateInstance().Play();

                                    //current game state is in deal cards
                                    gameState = DEAL_CARDS;
                                }
                            }
                            
                            //When the mouse location is within the reset button
                            if (MouseClicked(resetRec) == true)
                            {
                                //Play button click sound effect
                                buttonClick.CreateInstance().Play();

                                //The bet amount shown is nothing and the bet amount is $0
                                betAmountOutput = " ";
                                betAmount = 0;
                            }
                        }
                    }
                    break;
                case DEAL_CARDS:
                    {
                        //Setup timer for dealing cards
                        dealCardsTimer += (float)(gameTime.ElapsedGameTime.TotalSeconds);

                        //When the timer is under 4 seconds
                        if (dealCardsTimer < 4)
                        {
                            //When it is the players turn and the timer is at about 1 second perform the appropriate tasks
                            if (pTurn == true && (dealCardsTimer >= 0.9 && dealCardsTimer <= 1.1))
                            {
                                //player card is equal to the card at the top of the deck
                                pHand[numPCards] = deck[topOfDeck];

                                //Move card to appropriate location
                                MoveCards(pHand, numPCards);

                                //Increase the current top of deck and number of cards in player's hand
                                topOfDeck++;
                                numPCards++;

                                //Calculate the player's card total
                                pTotal = GetHandTotal(pHand, numPCards);

                                //Dealers turn to get a card
                                pTurn = false;
                            }
                            //When it's the dealers turn and the timer is at about 2 seconds peform the appropriate tasks
                            else if (pTurn == false && (dealCardsTimer >= 1.9 && dealCardsTimer <= 2.1))
                            {
                                //Dealer card is equal to the card at the top of the deck
                                dHand[numDCards] = deck[topOfDeck];

                                //Move card to appropriate location
                                MoveCards(dHand, numDCards);

                                //Increase the current top of deck and number of cards in dealer's hand
                                topOfDeck++;
                                numDCards++;

                                //Player's turn to get a card
                                pTurn = true;
                            }
                            //When it is the players turn and the timer is at about 3 second peform the appropriate tasks
                            else if (pTurn == true && (dealCardsTimer >= 2.9 && dealCardsTimer <= 3.1 ))
                            {
                                //Player card is equal to the card at the top of the deck
                                pHand[numPCards] = deck[topOfDeck];
                                
                                //Move card to appropriate location
                                MoveCards(pHand, numPCards);

                                //Increase the current top of deck and number of cards in player's hand
                                topOfDeck++;
                                numPCards++;

                                //Calculate the player's card total
                                pTotal = GetHandTotal(pHand, numPCards);

                                //Dealer's turn to get a card
                                pTurn = false;
                            }
                            //When it is the dealer's turn and the timer is at about 3 second peform the appropriate tasks
                            else if (pTurn == false && (dealCardsTimer >= 3.9 && dealCardsTimer <= 4.1))
                            {
                                //Dealer card is equal to the card at the top of the deck
                                dHand[numDCards] = deck[topOfDeck];

                                ////Move card to appropriate location
                                MoveCards(dHand, numDCards);

                                //Dealers card is shown face up
                                dHand[numDCards].isFaceUp = true;

                                //Increase the current top of deck and number of cards in dealer's hand
                                topOfDeck++;
                                numDCards++;

                                //Calculate the dealer's card total
                                dTotal = GetHandTotal(dHand, numDCards);

                                //PLayer's turn to get a card
                                pTurn = true;
                            }
                        }
                        //When the timer as greater than about 4 seconds
                        else if (dealCardsTimer > 4.1)
                        {
                            //Current game state is in player turn
                            gameState = PLAYER_TURN;
                        }

                        //When the player's total is equal to twenty one
                        if (pTotal == 21)
                        {
                            //Player has Blackjack 
                            gameResults[0] = true;
                        }
                    }
                    break;
                case PLAYER_TURN:
                    {
                        //When player has less cards than the maximum amount of cards peform the appropriate tasks
                        if (numPCards < MAX_CARDS)
                        {
                            //If double the bet amount is less than the wallet
                            //When the player has 2 or less cards
                            if (((betAmount * 2) <= wallet) && numPCards <= 2)
                            {
                                //The double down button is shown
                                doubleDownShow = true;
                            }
                            else
                            {
                                //The double down button is not shown
                                doubleDownShow = false;
                            }

                            //When the mouse is clicked track whether any buttons have been clicked
                            if (NewMouseClick())
                            {
                                //When the mouse location is within the hit button
                                if (MouseClicked(hitRec) == true)
                                {
                                    //Play button click sound effect
                                    buttonClick.CreateInstance().Play();

                                    //Player card is equal to the card at the top of the deck
                                    pHand[numPCards] = deck[topOfDeck];

                                    //Move card to appropriate location
                                    MoveCards(pHand, numPCards);

                                    //Increase the current top of the deck and the number of cards in the player's hand
                                    topOfDeck++;
                                    numPCards++;

                                    //Calculate the player's card total
                                    pTotal = GetHandTotal(pHand, numPCards);

                                    //When the player's total is greater than 21
                                    if (pTotal > 21)
                                    {
                                        //Player has lost and the current game state is in post game
                                        playerWin = false;
                                        gameState = POST_GAME;
                                    }
                                }
                                //When the mouse location is within the stand button
                                else if (MouseClicked(standRec) == true)
                                {
                                    //Play button click sound effect
                                    buttonClick.CreateInstance().Play();

                                    //Current game state is in dealer turn
                                    gameState = DEALER_TURN;
                                }

                                //When double down button track whether double down button is clicked
                                if (doubleDownShow == true)
                                {
                                    //When the mouse location is within the double down button
                                    if (MouseClicked(doubleDownRec) == true)
                                    {
                                        //Play button click sound effect
                                        buttonClick.CreateInstance().Play();

                                        //bet amount is doubled
                                        betAmount = betAmount * 2;

                                        //Player card is equal to the card at the top of the deck
                                        pHand[numPCards] = deck[topOfDeck];

                                        //Move card to appropriate location
                                        MoveCards(pHand, numPCards);

                                        //Increase the current top of the deck and the number of cards in the player's hand
                                        topOfDeck++;
                                        numPCards++;

                                        //Calculate the player's card total
                                        pTotal = GetHandTotal(pHand, numPCards);

                                        //When the player total is above 21
                                        if (pTotal > 21)
                                        {
                                            //Player has lost and the current game state is in post game
                                            playerWin = false;
                                            gameState = POST_GAME;
                                        }
                                        else
                                        {
                                            //Game state is now in the dealer turn
                                            gameState = DEALER_TURN;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case DEALER_TURN:
                    {
                        //Set the timer for dealer turn
                        dealerTurnTimer += (float)(gameTime.ElapsedGameTime.TotalSeconds);

                        //It is the player turn
                        pTurn = true;

                        //When the timer is at about 1 second peform the appropriate tasks
                        if (dealerTurnTimer >= 0.9 && dealerTurnTimer <= 1.1)
                        {
                            //Dealer's first bqrd is sl�wj
0 �         뿽 0  "`0(     ! d�a.d[0}.iqFacuUp = true;
           "        0   }
 "          ( *      0  //Thgn"tie!4imer h� avdAbout 2 secOnd pefrm0the ap0roPpiate tasiw
  (         `  `       eLsE if (`eqnerTurnT�mar >5 �.9 &" eealerTu�nTimdb 4}$2.1)
  "`       $`!   " (    {
 0   ` 0   $  !   b  $ " ( "/.Cylaulate tje deqleﯽ's�ciPd totil
          �?�     !     `    �dTOtal -`G�tHa.f�cta(,`Hant, OumTCards);(L
  "  !    "       *     !  �?/When!tha be`leR's tubn is esqcm |m`21
        ! 0�        " $     i&`(�Total(=1�20)
`"  ((                �    {
 � !    (   ����  (             ! //The deal�r har$Blackjaco
 $ (  b    0         `          galeRes5mtW[1Y = t�ﾽe�
           !       ("  � ,� }
   "       !!      $   "}
       "     �   ��� "  ﾵ(WheN the!tioer is !t `bout 3 sesonts, a.d 4hen tle goldowiog loGjc�O#cupS�wheN"t�e thmes iNcreases evmry sgbondM
   "  (" $         "   ulse iF tE!lerTurnTimez >=`(2/9!+ (cardCounu * 1)) &(dealerTupnTImer <} (3.1 *  c`rdOen� � 1))+
  �   �  !  (    !"     {2 !                ((( "  !//jen the"�ecldr totalpyr$�%cs+tha$0�
    $$         �  0 0   ` (﻽iF (dTotc���>(22)
    �0 8 !                  {
* (     `      $   " !`  0   ` a//When 4pe dealer is legs than 17
    p          $`  0 (          ib dTotal!< 17)
       "   `   �   !        !  �?�
!  0    ` " �      �   !            ?/Dealer ccrd �s!equal �o the berd at"the top of tle de{k   (0   ! �          8   $       0 dHand￿numDCards] = �eck[t/pOfDm!oU;

        (               � �    (    //L/ve ca0d`to thd apprnpri`te!locqtion*"         $ ! (`  0   !            �MordCardqhdHand((l}mDCarts)?
  !      (1" �           � 0 !      '/DeaNeR card is �hown fact(u0
                 !   �         !   `$HsnD[nulDCards].is�akeUp � true;-

     $0       $      p      (4      //In#re!s% the�cuzpent top on!the dgco `n` the!n}mber mg sards kj vhe player's*hanl
    0                 `     " 2     /'and"increase thm�aount &/b hou m`ny cirds`are a$ded tk the �ualer's `a.d
    ! �       b       ` !  "$   ` " cardAouj|++;
*$  0                   $   0$      #TopOf@ucK+;;�
   $  "`  $*      0   "     �   0   nuoDCavds++;
M
� 0(    !        $  *0  !� �        //Calcw|ata0|Je feale`.s card total
          ` 0 8!         l    `     dTotal = GetHanlPot￭,(�?�H￼n$,"numDSa2ds);

0 0    �,     0      $ ( 0 `  `0  $ //When the deader to�al ms grea�e than�21
 �       $   $   �    "  0 �      $ if �dTital > 29)
      `    !  `` (  $!  �$        ! {�"     `   ` &   $ !    (     (`(       !//PL`x%r h�s gon a�d the current gam-`s�ave is im pOst game
` i `  "       "   !    $       `  a    ￥layerWin 9 truu;              "   " � "0$   " ! 0 0  (  gcme4c|e ="POST_OAME9
 �$     `0$      &      `        ! }
 $      `   "$.�     0 "    2(  }
 &( �   $8  !4�(    (           /?When t(w �%ade� tgtaL is eQUil 4o ob le3{ uha~!57	
               "  ￹"       $  e�;e iG (dTktal >=$17)
            �`   0       $ "  ({ (!      $  $   0   �  0   �       !//When th￹`player(t�tal is greaTer(|	qn�g���iher tot���l
               0! `        (!   
 !1if (pTotcl!> lTmpal)
  (   `    !  "  4�    $     ` �    {
          � "  $�        $ !            +/@layer"has w/l
뿽     %             !"     8   $ 2!  0  pl!iezWin = true;
  `� `$  (            ! � `    !  ! }￭ $ !   !  !(�    $    0         �$//Gheo �?�jg play-p tot`l os less �haf dealeB`total
* ` 0      �   !( `(   $�  �   "    "else if xdPotan!<￹dTotal)
 0d    !      ￵      �              {
0  $   ` !   1!(�`     "         !     //Player has losu�
     	    ! `     0        ``" $*      tlayerWin <@falre?
(   0            "       $      "  0}

  (        "   �              뿽   $�./C7srgnt wamd!rtate is post$g�}e
              �                    "/aleRtate < PNBT_WA�E;
       ,  (   `    0  &   $ "   y
j  `             �     `    0}�    0     Ͽ=          ! }-
 �0  `� " (    !8   }
ﯽ       $            b�eik9
           "   ca{e POST_CAL�:         ( 0        y�  " $0   翽   !   �  0   //Vh�n rlayer t/tal a.f de!ler�toTah are mqu`l
 ($!   ��              `if%hpTotal =? dTotaL)
                    `   [
  40      $    $0 � !  `$ ! 'oGamm	has wkne �o#push "   `      $ (          "  g!me@usmlts[6] < true+
"   ��� �    `   "      4|
$         (     !b    2 el{m-M
0  $  !!     �( (   ! " {
      ($!   �h   #        $�//Game�has"not gonu to2pucl
 *    �    b�0&         ` $ 'am�Rdsultc[6] = fa,�e;M
  ���� �0 �  0 0 ￭!   "!  u
	
"�`         $     0     /�When ame�h`s ont gold to push peform the appro0rIa|e ta1cS    ﷽ !             `   	f (gameBesults{6] == false)
   &�   h      "  `    ${
$ (    ! ( !  !  0 "     ( $?/VHen`yt i� time To calcuhate the nev jqlan#e �f vhe walle|
J *       0 "       �      $ hf0(calcWanlet"9= tRue)
 0� $    (    ! &8 0�� "    {
         # 0     �         0 +//Wh�n p,cyu� biS won
  ! ` "   $ " !     b          !in hplaye2WIo } true)
 `      `          ((           �
     $  ￿!        $ �  0 `     `   //P<ayϿ�playur wo�![oUld�ag&ect
  �    �                 �    % 0  gaeuWon,CrEateAn{danc﻽()>A|Qy();

      0  0!`          0   $ $    0 //Wallqt has iNcreased by the bet amotn4
  `�   !     �� #  h     (0 ` !0    walle| = 7all%p + betAdomjt;

 )    �  `            "0 (       $� //When plcyer 4otql anf fealer topa| ara eqeal to gr l%sC than,r4
   "0    �     �  `   `  �     !! !!af (tD-6al <="210&& dTo|Al = 21)
� �       $"�    "`    �?�    !        y      !             "  0         $ $    //Gh�n"qlayer twtal iS greater u*al dea�er`|�t`lJ  �0  (뿽$"   !    ( !   0     (      `  if (pTo4Al ? dTot�l)
  ,   "  !             0 $              {
 �`             Ϸ�  "            !` $ (      //DealEr(has snn by poi�ts
   � "   $  �   $!   0  o�� 0�        $ �$   (GameRmsulus[4]0= tpue?
      ((    (     (   !   "    "       }
       0   (    0 `    "         � $}
  �    (       !   (  #  "    "@0 (/Wh$n 4he"dcler(total is greeter T�e�82!
   a1"       $           02   "  `` Alse0id 8dTotal�> 1)
* " �  $ (  `$      !          $    0{
B $    (       ��       $    x   � "(  //ThE De@l�r!(as busp
" (  $0  0   0     �    ! !   ` (      gameResults[3} =,tpUe;
 0$`   (   �  @ (  &  0  (    (  ! �
       ( �  0�  $,       ( ( !    !�/?The .ew allncg of th! 7Al,e4 is`|o n�!lnnGer be canculate 
          !   (  �  !   (`      "  �fahcWa-let�= Ͽ�ilse;
        $`       $    (        o
   8      ��(     `  � ($   `  ! //WheN p��yer hAc l/s|   $ !"  "    ```!$     *    "  elsg kf (playerWin ==0false)
 $   0"    ` 0@       ` `!    "0{
 q   !    (  p      !    ` �"       '-P�ay pl�ye� losT0{oulD"���bfebtJ"            �0 `0  0    `0  "     gam﻽Lost.CrgateMnstaoce().Ply(i;

 (   8     `       �  ` f � $(  0  /.Decra!se wi,let by bt coount
! !     " !0���      0 �  $      "    walnet 5 wil,up - bedAioqnu;
M
     (       $( $`  �      � �      //When ph�yer(tot�l and dwalGr$tmt`l are eqel to or less then 21
   (    0   ) 0"    0     !   `     if (pToTq,(,= 21 && d\Otal`t= 21)
       !       )    $ �     ￭ 0     {�
$     $�       (  " !0   & � `          //G(en �maxer tmtiL was �ess than dea�ez total
(      
&  (    !0 "       $  ( `  #�`0 if(pToval < dTotAl+
^       ""   @ ����d%   $         `  (  `  {
(0  !      !`                   "(          //Fea$er xas w+~ bi pointq
  � (     "                 �            #0�ameResultsZ5� = tsug;
  !翽             $ (        !"$ 0      }
       (       �   "  ( �           }
  (   0"   0  " "#     `    $      �/M`el pdayar to�cl is gremter than 21
           ` "        ! �     o�� `   ulse�if (pTotal0: 21(
 0         `(�   �         � $   0` {
    `   $"� $          $`( $      ((  ( /?The pLayer has b}su
  0(          `      @!     0    "" !   gemmREs}lts[2]0= true
   "  $�           $       "      `$

  ( "    (  `  0    "               oThe �e￼ balance nf v�e w`llet ib(to ntodbe calgud�def(    !  $ 3! ! 0   ￭    0$  $  0    cclcUanlGt(} False;
(     (   `    ` 0 �(     * (( =
       `    p    0    8"�  }
 " o��     0!   !  � ""  }
B  $!     0    p   "     
    $       0         ﷽0//Wye. a!Oowse(has bme/�cl�ckee!and the0mouse ln�etimn is wi4hin uhe nexv cetton
  8        (   "!    `  if (NewMoUseC|ick)) == true && Mouseclicked(ngxtRec) -= true)�
      �(   0     0   (  {
          0        !    "   //Plai b}tton clmck soufd effdcu
    %�   !  $"              buttonClicknCreateinstance)).P�ay();
	
 ` `          "  �      !   �/Reset Game
    "    ! ,       "    `   R%setGame()+
        $               }-
!       `"   $      }
                    break;M
 !              cAse END_GAME:
     $              {
                        //Set00ime2 For whan(tn epit 'ame
      " 0              !axitaieTimmr +="(flo`t)(gameTime.ElapsedGameTime.TotalSgco.ds);
 0     b    (          //When t�% tim%r hqs reached about 3 seconds
         !              if (dXiTameTimur >= 2.9 && exitGemeTimEv"<= 3.1)J       $0           $   {	
               (            //Exit program
   !      � (0              E8ht();
  $       �             }
      ( !           }
 (       `          break;
   b  (   ( }
  �         jase.Updcte(gaMeTime);
  0 (   }J       !protected override voi| Draw)G�meTkme wameTIme)
� `   ! z
( (�   h    GraphicsDavicu.S�aab(Color.CornflowerB�ue);

  0      0  s`riteBAtcj.Begyn();

    0      //Draw the fkllOwing grarhics according to the game stau%
            swjtcl((g!-}State)            {
    0  0�  �    casm PREGA]E:
0 ! (       $       {
    0                   //Dpaw th� �regamE baakground�J            `   `       spriteBatch.Draw(pregamgBgImg, roRec, Color.Ghite);

 "             $        //When the usur's wal|et ks aBove $0
   0(�             `    if (q!llut > 0)
     �                  {             ` !      "     /'Draw the fet button
   $                     !  sprite@avkh.Drew(be�ButtonIog, betRes, Color.Whi`e);
                        }            "   �       gls�
               (        ;
                   !    (  $//DraW a vransparent beT button
                   `   4    spriteB!tc�.Dr!w(b�tButtonImg, be�Rec, Color.W(ite * 0.5f);
    $  �          `  $  }
                        +'Draw pregame butt�ns
 !    $          #      sp�iteBavch.Draw(exitButtonImg, exitRec, Cgl/r.W�ite);
         "             (spriteBatch,Draw(instrectionsButvonImg, �nstr�ctaonsButtonRe#, Cglor.Whitd);

      $          $      //Traw the wallet l�bel and 4(e wanhet bala�ce
          �         (!  spriteBatch.DrawS|ring(labemFont, walletLabel + walhet, wanle|Loc, C�lor.White,;
      !      $     $}�                    `rmak;M
      !         case(KOSTRUCTIONS:
       !    (   ( $ {
   "       "   !" (�    //D2as instrustio�r ymages� (                     striteBatch.Draw(inwtructionsBgMMg, izstr�ctionsBgRec, Color.White); $  `            $      spriteJctch&Draw�baciBupdonKmg, bqCkButu��Rec, C�l)r.Uhi|g);*    )    0          }	
            !  !    break;* �     �        case BETTING:
   "   $  0         {
         "!      �      //Draw�rettino images
 0           (     �    s�riteBatch.Draw(bettingBgImg, cgRec, Col/r.Whit%);
         `     (  `     spri4eBatch.Draw(resetButtonIMo, resetRec, Golor.White);
  $          `      "  �//D�a� rettinc teq4s
         �  `   �       spriteBatsh.DrawStrIng(labelFont, wahlutLabel + wallet, walletLoc, Color.White);
                    0   q�ritEBatch.DrawQtr)ng(labelfont� currentBetutpUt, cu�ren4BetLmc, Color.White);

                  ,`    /.She.!the bet is$~el)d
                        if (isBetValid == 4rue)$   `               `  0{
  ` �  `  �                 //D2aw the bet"amount pext in gree�( and the b%t "uwton
    " `         0           spriteJatch.DrawString(|abelFont, dollarSagn`+ bet@mountOetp5t< betLnc,�Bolor.Wreen);
    �             (      "( spvite@atch,Draw(budButtonI�g, bettingJetRec- Color.Whide!;
"   !                 $ }
(                  0!   elwe
   !             $      {
       �  �        00   (   //Draw the bet amount text in red, and thm transparent�bet button
       $         0  !       wpriteBatch.DrawString(labelFonu, dollarSign + retAoo�ntOttpud, betLoc, Coloz.Red);J       �                    wpriteBa4cj�$raw(`etButtonIig, bettincBetRec, Colos.Whita : 0.5f);
                      ! }

 !    !  h(  !          //Br all�numberS �etween 4 and numbers.Length
 !        �        �  ( f/r (int i = 8; i  numbers.Length; i+)
           &            {�                            //Draw0white boxes iNficated by 4he numjer i
  �          �              sqritdBatch>Dsaw(whiteBoxImg, whi|dRoxRdc[i],"Cklor.Wh�te);

     (            0         //When the"use is hovering ov�r the box indmcated!by the n�mber i
                            if (BgzHmvar(i) ==(true)
      `     0       $    !  {
          `       `  (          //Draw all the whit� boxes
($                  " "     !   spriteBAtch.Dr`7(whiteB�pImo, whiteBoxRec[i], new Co�osh126, 22 , 252));
          `      `�        �}

  #               !         //Fraw a|l thu numbers indicatet by tje oumBer �
       &    0!     `  $     spriteR!tch.Dr!wStv).g*NumberFont,&number�[i], numbmrsLoc[i]$ Cglor.Black);
          !   "      0 $|
   $               0}                 �  b�eak?
  0  0  $    �! cace DEALCARDS;
$     !             {
    �               ! ( //Draw backgr�und mmage
  (   "    "   !   (` � spriteJauch.Draw(gqmepla}BgIlg, b'Rec,"Cnlo2<Whit%	;

          !             //Draw deal cards taxt3
0       (               spriteFatch.DrawStringhlabelFnt, betAmountabel$+ betAmount, betAeou~tLa`enLoc, Color>Yellow!;�
           !0  "        sprite@atch.Dzaw[triNghlAbelFont, walletLabeh� walLat, willetLab-lLoc, C�lor.Yel,ow);-
            $    "`     spriteBctch>DrasString*labelFont, dealerTotalLeben$+$dTotal, dealdrTotclLabe,Loc, color.Whitg);
   "`  �                spriteBat�h�DrawString(larelFo�t, p|�yerTotalLabel + pToTal, pl!yerTo|clLabelLoc, Color.White);
  "  (        !         spriteBatch.DrawSvvinghlabelFont, "Deck> "�+ c!rdsInDeck, numCar$sLo�, Bklmr.White);

          "`           0//�oR ald!tie lumbers between 0 and CARDS	
    0   $     0         for (ilt i = 0; h < CARDS; i/+)
   (      ( �           {
                       !    //Draw all the casds if dhe deck indic!ted ry the k
  #          !   !$  `      dea+[i].Draw(striteBatch);O
           `    0     " }
      $    `        }
   `         0` &0! break;
                case�P\AYER_TURN:
             �      {
                0       ./Draw background image
      $ $   0 !    ( !  rpriteBetch.DrawgameplaqBgImg, bg�ecl Color.Whitm);
                        //When the0doufle down ic no� do be shOwN
   �   0                if (dmubleDownShow <= false)
(                   `   {
"         �$        "   $   //Drau`trensparent double dkwn b5tton
                      �     spriteBatchnDraw(doubldDownButponImg, doubleDow.REc, Colob.Whyte * 0.5f	;
            `   (     ` }
          `             else�                     `  {-   `  `   !                 //Draw double down buttoN
               �            spriteBe�ch.D2aw(doubleDownButtonImg, $oqbleDownZec, Color.white)?
        $(   `  �  (   �}

                        //Draw playe2 duzn buttons
 (                  0   sprItm@a�ch.Drag HitButtonImg, hitRec- Codor.White);
          0       "     spriteatch.Draw(standButtonHmg,`standRdc, Color.White);

                        //D�a player turn t�x|s
   ( �                 �spriteB�ucH.FrawString(labElFknt, beuAmountLabel + betamnunt$ bet@mountLabelLoc, Colkr.Yellow);
 `                      {prite@atchnDraGCtrijg(labelFont, wallet\a`en + wa�het, walletLabelLoc, Color.Yellow);
     � � $           !  sprIteBatch.DrawStpang(lab%lFont� leale�TgtalL�bel(+"DTotal, fealerTotaNLabe~Loc( Cklor.White);
    0  �     �   �   !  spriteBatch.DrawString(labelFont, playerTotalLabel + pTotal, playerTotalLabelLoc, Color.White);
                        spriteBatch.DrawString(labelFont, "Deck: " + cardsInDeck, numCardsLoc, Color.White);

                        //For all the numbers between 0 and CARDS
                        for (int i = 0; i < CARDS; i++)
                        {
                            //Draw all the cards in the deck indicated by the i
                            deck[i].Draw(spriteBatch);
                        }
                    }
                    break;
                case DEALER_TURN:
                    {
                        //Draw the dealer turn background
                        spriteBatch.Draw(gameplayBgImg, bgRec, Color.White);

                        //Draw transparent hit, stand and double down buttons
                        spriteBatch.Draw(doubleDownButtonImg, doubleDownRec, Color.White * 0.5f);
                        spriteBatch.Draw(hitButtonImg, hitRec, Color.White * 0.5f);
                        spriteBatch.Draw(standButtonImg, standRec, Color.White * 0.5f);

                        //Draw dealer turn texts
                        spriteBatch.DrawString(labelFont, betAmountLabel + betAmount, betAmountLabelLoc, Color.Yellow);
                        spriteBatch.DrawString(labelFont, walletLabel + wallet, walletLabelLoc, Color.Yellow);
                        spriteBatch.DrawString(labelFont, dealerTotalLabel + dTotal, dealerTotalLabelLoc, Color.White);
                        spriteBatch.DrawString(labelFont, playerTotalLabel + pTotal, playerTotalLabelLoc, Color.White);
                        spriteBatch.DrawString(labelFont, "Deck: " + cardsInDeck, numCardsLoc, Color.White);

                        //For all the numbers between 0 and CARDS
                        for (int i = 0; i < CARDS; i++)
                        {
                            //Draw all the cards in the deck indicated by the i
                            deck[i].Draw(spriteBatch);
                        }
                    }
                    break;
                case POST_GAME:
                    {
                        //Draw post game background
                        spriteBatch.Draw(gameplayBgImg, bgRec, Color.White);
                        
                        //For all the numbers between 0 and CARDS
                        for (int i = 0; i < CARDS; i++)
                        {
                            //Draw all the cards in the deck indicated by the i
                            deck[i].Draw(spriteBatch);
                        }

                        //Draw post game images
                        spriteBatch.Draw(whitePageImg, whitePageRec, new Color(0, 0, 0, 180) * 0.8f);
                        spriteBatch.Draw(nextButtonImg, nextRec, Color.White);

                        //Draw post game texts
                        spriteBatch.DrawString(numberFont, dealerLabel + dTotal, dealerLabelPostGameLoc, Color.White);
                        spriteBatch.DrawString(numberFont, playerLabel + pTotal, playerLabelPostGameLoc, Color.White);
                        spriteBatch.DrawString(resultsFont, resultsLabel, resultLabelLoc, Color.Yellow);
                        spriteBatch.DrawString(numberFont, betPostGameLabel, betLabelPostGameLoc, Color.White);
                        spriteBatch.DrawString(numberFont, walletLabel + wallet, walletPostGameLoc, Color.Blue);

                        //When the result of the game is a push 
                        if (gameResults[6] == true)
                        {
                            //Draw text for the game ended in push
                            spriteBatch.DrawString(numberFont, "(" + dollarSign + betAmount + ")", betPostGameLoc, Color.White);
                            spriteBatch.DrawString(numberFont, pushOutput, outcomeLabelLoc, Color.Blue);
                        }
                        //When the player has won 
                        else if (playerWin == false)
                        {
                            //Draw the text for when the player has won in Red
                            spriteBatch.DrawString(numberFont, "(" + minusSign + "$" + betAmount + ")", betPostGameLoc, Color.Red);

                            //When the Dealer has Blackjack
                            if (gameResults[1] == true)
                            {
                                //Draw Dealer has Blackjack text
                                spriteBatch.DrawString(numberFont, dBlackJackOutput, outcomeLabelLoc, Color.Blue);
                            }
                            //When the dealer has won by points
                            else if (gameResults[5] == true)
                            {
                                //Draw dealer has won by points text
                                spriteBatch.DrawString(numberFont, dWinPointsOutput, outcomeLabelLoc, Color.Blue);
                            }
                            //When the player has busted
                            else if (gameResults[2] == true)
                            {
                                //Draw player has busted text
                                spriteBatch.DrawString(numberFont, pBustOutput, outcomeLabelLoc, Color.Blue);
                            }
                        }
                        //When the player has won
                        else if (playerWin == true)
                        {
                            //Draw text for when the player has won in Green
                            spriteBatch.DrawString(numberFont, "(" + plusSign + dollarSign + betAmount + ")", betPostGameLoc, Color.Green);

                            //When player has Blackjack
                            if (gameResults[0] == true)
                            {
                                //Draw text for player has Blackjack
                                spriteBatch.DrawString(numberFont, pBlackJackOutput, outcomeLabelLoc, Color.Blue);
                            }
                            //When player has won by points
                            else if (gameResults[4] == true)
                            {
                                //Draw text for player has won by points
                                spriteBatch.DrawString(numberFont, pWinPointsOutput, outcomeLabelLoc, Color.Blue);
                            }
                            //When dealer has busted
                            else if (gameResults[3] == true)
                            {
                                //Draw text for when dealer has busted
                                spriteBatch.DrawString(numberFont, dBustOutput, outcomeLabelLoc, Color.Blue);
                            }
                        }
                    }
                    break;
                case END_GAME:
                    {
                        //Draw end game background
                        spriteBatch.Draw(postgameBgImg, bgRec, Color.White);

                        //Draw end game texts
                        spriteBatch.DrawString(labelFont, endingStatement, endingStatementLoc, Color.White);
                        spriteBatch.DrawString(labelFont, endingStatement2 + wallet, endingStatementLoc2, Color.White);
                    }
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
        
        //Pre: N/A
        //Post: N/A
        //Description: Creates the initial deck of cards using the number of suits and the number of cards in each suit
        private void CreateDeck()
        {
            //count tracks the number of cards created
            int count = 0;

            //For every suit, create each card from Ace to King
            for (int i = 0; i < Card.NUM_SUITS; i++)
            {
                for (int j = 0; j < Card.CARDS_IN_SUIT; j++)
                {
                    //Create and add the new card to the deck array
                    deck[count] = new Card(deckImg, faceDownImg,deckRec, i, j);

                    //increase count
                    count++;
                }
            }
        }
        
        //Pre: The number of times the deck is to be shuffled
        //Post: N/A
        //Desc: Loop numShuggles times and generate 2 random numbers from 0 and length of deck 
        //and swapping the elements in the deck to those elements
        private void ShuffleDeck(int numShuffles)
        {
            //Set place holder
            Card randomNumPlaceholder;

            //For every number for 0 to numShuffles the deck is shuffled
            for (int i = 0; i <= numShuffles; i++)
            {
                //Set random numbers to a random number between 0 and deck.Length
                randomNum = rng.Next(0, deck.Length);
                randomNum2 = rng.Next(0, deck.Length);

                //Set place holder to random card from deck
                randomNumPlaceholder = deck[randomNum];

                //Have one random card element equal another random card element in the deck
                deck[randomNum] = deck[randomNum2];
                deck[randomNum2] = randomNumPlaceholder;
            }
        }
        
        //Pre: Card hand, and an int for the number of cards in the hand
        //Post:Returns the player's or dealer's card total
        //Desc: Calculates the hand total using the value of the symbol of the card, Also
        //determines whether the value of the ace is a 1 or 11
        private int GetHandTotal(Card[] hand, int numCardsInHand)
        {
            //Set variables for aceCount, total, value, and countNumStart
            int aceCount = 0;
            int total = 0;
            int value = 0;
            int countNumStart;

            //When it is the player's turn
            if (pTurn == true)
            {
                //countNumStart is equal to 0
                //This means that all the cards will be accounted for when calculating the total
                countNumStart = 0;
            }
            else
            {
                //Set countNumStart to 0
                //This means that the first card of the hand will not be accounted for when calculating the total
                countNumStart = 1;
            }

            //For all numbers between countNumStart and the numCardsInHand
            for (int i = countNumStart; i < numCardsInHand; i++)
            {
                //When the symbol of the card is less than or equal to 9
                if ( hand[i].symbol <= 9)
                {
                    //Value of the card is the symbol of the card plus 1
                    value = hand[i].symbol + 1;

                    //When the symbol of the card is 0
                    if (hand[i].symbol == 0)
                    {
                        //Increase the number of aces
                        aceCount++;
                    }
                }
                //When the symbol of the card is greater than or equal to 10
                else if (hand[i].symbol >= 10)
                {
                    //Value of the card is 10
                    value = 10;
                }

                //Add the value of the card to the total
                total = total + value;

            }

            //For every number between 0 and aceCount
            for (int i = 0; i < aceCount; i++)
            {
                //If the total is less than or equal to 21 when 10 is added to the total
                //then perform the appropriate tasks
                if ((total + 10) <= 21)
                {
                    //Increase total by 10
                    total += 10;
                }
            }
            
            //Return the value of total
            return total;
        }
        
        //Pre:N/A
        //Post:N/A
        //Desc:Determines whether the mouse has been clicked or not
        private bool NewMouseClick()
        {
            //When the left button of the mouse is pressed and the left button was not previously pressed
            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton != ButtonState.Pressed)
            {
                //A new mouse click has occured
                return true;
            }
            
            //No new mouse click has occured
            return false;
        }

        //Pre: Rectangle
        //Post: Returns a bool variable 
        //Desc: Determines whether the x and y coordinates of the mouse where
        //within the x and y coordinates of the Rectangle being tested
        private bool MouseClicked(Rectangle box)
        {
            //When the mouses location is within the x,y coordinates, length and width of the box
            if (mouse.X <= box.X + box.Width && mouse.X >= box.X && mouse.Y <= box.Y + box.Height && mouse.Y >= box.Y)
            {
                //Mouse was within the box
                return true;
            }
            else
            {
                //Mouse was not within the box
                return false;
            }
        }
        
        //Pre: array element from the whiteBoxRec array
        //Post: bool variable for whether mouse is hovering over box
        //Description: Determines whether the user is hovering over the white boxes
        //in the betting game state
        private bool BoxHover(int arrayNum)
        {
            //Call subprogram for whether the user mouse is hovering over the white box
            //When mouse is hovering over white box is true
            if (MouseClicked(whiteBoxRec[arrayNum]) == true)
            {
                //return true
                return true;
            }
            else
            {
                //return false
                return false;
            }
        }

        //Pre:array element from the whiteBoxRec array
        //Post: N/A
        //Description:Converts the string of the numbers chosen by the user to 
        //a string after the user has finalized their bet
        private void BoxClicked(int arrayNum)
        {
            //Convert arrayNum to string
            string arrayNumOutput = Convert.ToString(arrayNum);

            //The arrayNum is added on to the bet amount text
            betAmountOutput = betAmountOutput + arrayNum;

            //The text is converted to an integer
            Int32.TryParse(betAmountOutput, out betAmount);
        }

        //Pre: Card hand, number of cards in the hand
        //Post: N/A
        //Description: Moves card to appropriate location from the location when
        //in the deck depending on whether it is the player's or dealer's turn to recieve a card
        private void MoveCards(Card[] hand, int CardsInHand)
        {
            //Play soundeffect for an instance
            dealingCards.CreateInstance().Play();
            
            //Decrease the amount of cards in the deck
            cardsInDeck--;

            //When the game state is in deal cards or player turn
            if (gameState == DEAL_CARDS || gameState == PLAYER_TURN)
            {
                //When it is the player's turn
                if (pTurn == true)
                {
                    //The card is shown face up
                    hand[CardsInHand].isFaceUp = true;

                    //Card's y location is moved to the predetermined y location
                    hand[CardsInHand].dest.Y = PY_INITIAL;
                }
                //When it is not the players turn
                else if (pTurn == false)
                {
                    //Card's y location is moved to the predetermined y location
                    hand[CardsInHand].dest.Y = DY_INITIAL;
                }
            }
            //When the game state is in  dealer turn
            else if (gameState == DEALER_TURN)
            {
                //Card's y location is moved to the predetermined y location
                hand[CardsInHand].dest.Y = DY_INITIAL;
            }

            //Cards original x location is moved to the predetermined x location
            //plus a certain amount to the right depending on the number of cards in the hand
            hand[CardsInHand].dest.X = X_INITIAL + (CardsInHand * SPACER);
        }

        //Pre: N/A
        //Post: N/A
        //Description: Set all variables used throughout the game back to their original value
        private void ResetGame()
        {
            //Set all variables used throughout the game back to their original value
            betAmountOutput = "";
            betAmount = 0;
            numPCards = 0;
            numDCards = 0;
            topOfDeck = 0;
            pTotal = 0;
            dTotal = 0;
            dealCardsTimer = 0;
            dealerTurnTimer = 0;
            cardCount = 0;
            calcWallet = true;
            resetGame = true;
            cardsInDeck = 52;
            gameState = PREGAME;

            //For all the numbers between 0 and gameResults.Length, set bool variable value to false
            for (int i = 0; i < gameResults.Length; i ++)
            {
                //gameResult is false
                //As the game is set to the begining where the result is still unknown
                gameResults[i] = false;
            }
        }
    }
}