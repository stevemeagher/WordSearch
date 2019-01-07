Word Search
===========

The input to this application (**WordSearch**) is a text file consisting of a series of words (search words) and an **N x N** grid of characters.  It displays the coordinates of the grid locations at which the search words are found.

When run, **WordSearch** will display, in alphanumeric order, a list of up to 9 files found in the **wordsearch/puzzles** directory.  With the supplied puzzles, the list appears as:

```
---- Word Search ----
(1) dark.txt
(2) expansive.txt
(3) highlyillogical.txt
(4) repetitive.txt
(5) strange.txt

Select puzzle number: 
```

By pressing a listed number, the search words and grid for the selected puzzle are displayed. If puzzle 3 was selected...

```
BONES,KHAN,KIRK,SCOTTY,SPOCK,SULU,UHURA

U M K H U L K I N V J O C W E
L L S H K Z Z W Z C G J U Y G
H S U P J P R J D H S B X T G
B R J S O E Q E T I K K G L E
A Y O A G C I R D Q H R T C D
S C O T T Y K Z R E P P X P F
B L Q S L N E E E V U L F M Z
O K R I K A M M R M F B A P P
N U I I Y H Q M E M Q R Y F S
E Y Z Y G K Q J P C Q W Y A K
S J F Z M Q I B D B E M K W D
T G L B H C B E C H T O Y I K
O J Y E U L N C C L Y B Z U H
W Z M I S U K U R B I D U X S
K Y L B Q Q P M D F C K E A B

(1) Show solution
(2) Enter a search word
(3) Select another puzzle
(4) Exit

Enter selection:

```

Via the menu you can then view the fully solved puzzle including search word coordinates, enter individual search words (either the supplied search words or any character combination), select a different puzzle or exit the application.


Running WordSearch
------------------

To download **WordSearch**, run the tests and execute the application:

1. Ensure your target environment has [Visual Studio Code](https://code.visualstudio.com/download) and the [.Net Core SDK 2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2) installed 
2. Download and expand the ZIP file
2. Open VS Code (remaining steps are executed in VS Code)
3. Open the root application folder for **WordSearch** e.g. WordSearch-master
4. Open the terminal and build the application:
```
dotnet build
```
5. Change to the ``tests`` directory and run the **WordSearch** tests:
```
cd tests
dotnet test
```
6. Change to the consoleapp directory and run the console application:
```
cd ../src/consoleapp
dotnet run
```
7. Follow the on-screen instructions



My Approach to the Problem
--------------------------

When I first considered a solution to this kata, I thought about the possibility of creating a string array and writing code to search the array by looking for the first character of the search word, and, once found, checking its eight closest neighbours to determine if any matched the second character of the search word and so on.  

Then I realized that I could create eight strings to represent the search grid in the eight directions that needed to be searched (up, down, left, right, top-left to bottom right, bottom-right to top-left, top-right to bottom-left and bottom-left to top-right), and simply use .IndexOf to find the string that contained the search word.  There would then need to be a conversion from the string index position to coordinates in the grid.  That was the approach I went with.

Therefore, the complexity, as much as there is, is contained in the code that generates the search strings and the mapping back from the string index to the grid.

