Dokumentation: github/wiki

!The game only works on http it doesn't have implementation for https!
Entry point: http://localhost
in the finalsd folder: docker compose -f docker-compose.yml -f docker-compose.override.yml up

Short description of the game:
	1. The user logs in or registers
	2. The user have two ways to start playing
		2.1 They either create a new game by choosing how many rounds they want to play and if they want to make their game public witch means letting every user see their game's id making them able to enter the game
		2.2 Or choose an id from the public games list and enter an alredy existing room
	3. Play the game
		-Before starting the game the players are able to message each other and draw together
		-After clicking the start game button one player is going to be the painter and the others ar egoing to be guessers
		-The painter has to draw the given world and the others have to guess what the drawing is. The painter finishes if everyone guessed right or if the painter decides to skip
		-If someone guesses the painting right they get a point
		-A round is over when every palyer drawn once
		-The game is over when every round is over the winner is the player with the most points a tie is possible
	4. After the game is over the players return to their profile page where their statistics are updated with thier last games data and there they can start a new game

User -->ApiGateway -->Auth ms /Jatek ms/Statisztika ms

The solution contains 4 microservice, Integration tests for the Auth and Jatek microservice and UI tests.

The 4 microservice:

	ApiGateway(port: 80):
		Forwards the calls to the right microservice.
		
	Auth(port:5011)
		User management
		Endpoints:
			POST:
				Register(form-data: UserName,Password,ConfirmPassword)
				Login(form-data: UserName,Password)
			GET
				Register(): Returns to Register page
				Login(): Returns to Login page
				Logout(): Returns to main page
				Profile(): Gets statistics and Returns to profile page
				
	Jatek(port:5005)
		Game management
		Endpoints:
			POST:
				StartGame(form-data: Rounds(int),Public(bool)): Creates a room with the given data then Redirects to the Game page
				Send(body: PlayerId(string), IsWon(bool), Points(int)): Sends the game data to the Statisztika ms with MassTransit
			GET:
				EnterRoom(url: roomId): If the a room with the given id exists enters the room and redirects to the game page
				RemovePlayer(): Removes the user from the room they are in then redirects to the game creation page
				GetPainterFinished(url:id): Returns if every guesser in the room guessed right meaning the round is over
				GetRole(url: id): Returns the given useres role(painter or guesser) in a game
					if a user with the given id doesn't exist throws exception
		SignalR
			Manages the in game chat, drawing, getting the new word and the order of the players
			
	Statisztika(port:5031)
		Statistics management
		Endpoints:
			GET:
				GetStatisticsById(url: id): Returns the given useres statistics
					if a user with the given id doesn't exist throws exception
		Consumer:
			Calculates the statistics from the arriving data  than saves it