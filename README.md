# CrimeChronicles

# Introduction

Step into the shoes of Dr John Watson in "Crime Chronicles," an immersive detective adventure game where players must solve a perplexing murder mystery with the assistance of the brilliant Sherlock Holmes and the diligent Inspector Lestrade. Using advanced AI, every scenario and every interaction with suspects, Inspector Lestrade, and Sherlock Holmes is dynamically generated, ensuring a unique experience every time.

Meet the five suspects, each with a complex background. Engage in deep, branching dialogues powered by OpenAI’s language model, where each response can uncover new information or raise further questions. When you feel confident in your deductions, make your final accusation. 

Can you uncover the truth behind the murder and bring the murderer to light?

# Table of Content

[How to Play using Game UI]([link](https://github.com/Rohan-J123/CrimeChronicles#how-to-play-using-game-ui))
[How to Play using Terminal]([link](https://github.com/Rohan-J123/CrimeChronicles#how-to-play-using-terminal))
[Target Audience for Crime Chronicles]([link](https://github.com/Rohan-J123/CrimeChronicles#target-audience-for-crime-chronicles))
[Architectural Design]([link](https://github.com/Rohan-J123/CrimeChronicles#architectural-design))
[FrameWork]([link](https://github.com/Rohan-J123/CrimeChronicles#framework))

# How to Play using Game UI

https://github.com/Rohan-J123/CrimeChronicles/assets/139244765/840a00f3-7ade-4a08-86eb-ae2c0ed757fc

### Download and Installation:

1. Download the project from Crime Chronicles on [itch.io.](https://rohan-j123.itch.io/crime-chronicles)
2. Extract the downloaded ZIP file.
3. Run the application from the extracted folder.

### Setting Up the Game:

1. Obtain an OpenAI API key from the OpenAI website.
2. Launch the game and add your API key using the first button on the home page.
3. Start the server using the appropriate button. The "Start Game" button will turn green once a valid server connection is established.
4. Press "Start Game" to begin your adventure.

### Server Issues and Alternative Setup:

If the game fails to host a server, follow these steps:
1. Download the GitHub repository from Crime Chronicles on GitHub.
2. Navigate to [directory](https://github.com/Rohan-J123/CrimeChronicles/tree/main/CrimeChronicles/Assets/StreamingAssets/Scripts/contextful_parsing) and execute the run.sh file using bash or run the commands listed in the file individually in the terminal. This will start a server using Docker.
3. Return to the game and press the "Start Game" button.

### Gameplay Overview:

The Game starts with Inspector Lestrade greeting you at the crime scene and telling you about the murder. Then, you can interrogate five suspects related to the victim to uncover something new or raise further questions. You can also ask for help from Inspector Lestrade and Sherlock Holmes, who are sources of unbiased information and act as hints throughout the game. At the end of your interrogation, accuse one of the suspects. This will prompt Sherlock Holmes to reveal who the actual murderer is and whether you are right or wrong. Then, play again:)

### Privacy Notice:

All API keys and text files are stored locally. No user information is collected.

# How to Play using Terminal

https://github.com/Rohan-J123/CrimeChronicles/assets/139244765/d0c9342d-4579-40c7-b3b9-c9da3dbfd389

### Download the GitHub Repository:

1. Download the repository to your local machine.
2. Navigate to the [required directory](https://github.com/Rohan-J123/CrimeChronicles/tree/main/CrimeChronicles/Assets/StreamingAssets/Scripts/contextful_parsing).
3. Execute the run.sh file using bash or run the commands listed in the file individually in the terminal. This will initialize the server using Docker.

### Connect to the Server:

1. Run the retrieve.py file. This will establish a connection to the server.

### Gameplay Instructions:

1. Interrogate Suspects:
Pose questions to characters like "Sherlock Holmes", "Inspector", or any of the other suspects by using their names.
2. Determine the Murderer:
Once you have gathered enough information, query "Sherlock Holmes Reveal" to find out who the murderer is.

# Target Audience for Crime Chronicles
Crime Chronicles is designed for a diverse range of players, including:

### Mystery Enthusiasts:
Fans of detective stories and murder mysteries will enjoy the engaging narrative and the process of solving a crime through interrogation and deduction.

### Puzzle Solvers:
Players who enjoy solving puzzles and critical thinking challenges will find the game's interrogative and analytical elements appealing.

### Interactive Story Lovers:
Those who appreciate interactive fiction and narrative-driven games will be drawn to the immersive storytelling and character interactions.

### Fans of Sherlock Holmes:
Admirers of the Sherlock Holmes universe will appreciate the inclusion of familiar characters like Sherlock Holmes and Inspector Lestrade, which adds a layer of authenticity and excitement.

### Casual Gamers:
With intuitive gameplay mechanics and a straightforward setup, the game is accessible to casual gamers looking for a fun and engaging way to pass the time.

### Educational Use:
Educators and trainers may use the game as a tool to develop critical thinking, problem-solving, and analytical skills in an entertaining format.

Whether you are a seasoned detective or new to the genre, Crime Chronicles offers an engaging and thought-provoking experience for all players.

# Architectural Design

![CrimeChroniclesArchitecturalDesign](https://github.com/user-attachments/assets/602a5d9d-29cc-4bc6-8259-e53e2aea8ffb)


# Framework
### Integration with Pathway Library and OpenAI:
Crime Chronicles leverages the Pathway library for its Retrieval-Augmented Generation (RAG) functionalities to interact with the OpenAI API. The game initializes by starting a server using Docker, which facilitates queries to the OpenAI API. This server simulates game characters, responding based on scenarios generated and previous conversations. The Pathway library processes and tokenizes these interactions.

### Server Initialization and Data Handling:
The game starts a server using Docker, enabling seamless communication with the OpenAI API.
This server functions as the in-game characters, responding dynamically based on pre-defined scenarios and prior interactions.

### Scenario Management:
A scenario is generated using OpenAI and langChain and loaded into the data folder upon server creation.
Players engage with the server as John Watson, conversing with suspects, Sherlock Holmes, or Inspector Lestrade.
All conversations are stored in the data folder, thus providing Pathway with a dynamic data source during runtime.

### Dynamic Interaction:
Pathway’s RAG capabilities facilitate dynamic scenario changes. When a new scenario is loaded, all previous conversations are reset.
Thus, the same server is reused for queries with new suspects, ensuring fresh and innovative interactions.

### Ensuring Fresh and Consistent Gameplay:
Each interaction and conversation within the game is generated by OpenAI, providing unique and innovative experiences for players.
Pathway analyzes and tokenizes conversations to maintain character consistency and coherence across different scenarios.

By integrating [Pathway's](https://pathway.us12.list-manage.com/track/click?u=70279b5d6def5ed7bf629c1f3&id=aca085ed7e&e=f50acf6b23) advanced RAG functionalities with OpenAI's capabilities, Crime Chronicles offers an engaging and dynamic gaming experience, ensuring each playthrough is unique and immersive.

Enjoy your immersive investigative experience with Crime Chronicles!





