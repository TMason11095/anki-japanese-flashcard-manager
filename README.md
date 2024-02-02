# Anki Japanese Flashcard Manager
C# project to take an Anki collection.anki2 database file and rearrange Japanese decks based on what cards you're currently learning and have already learned.

## ankiBindingTags.json
JSON file used to link Anki Tags from the .anki2 file to the program.
* deckTag - Used to identify the deck name from the deck's description. (ex. "deck:")  
![image](https://github.com/TMason11095/anki-japanese-flashcard-manager/assets/134988352/48edb903-1fdb-439c-a71c-de714f4b7380)
* resourceDecks - Used to map resource decks using the deckTag.
  * kanji - Name of the deck that holds kanji cards that may be components of kanji you're currently learning. (ex. "KanjiResource")
    * Example use of deck: If you're learning the kanji 飲 (drink) and the kanji 食 (food) and 欠 (lack) are in this deck, then the program will move those cards into your learning deck. (The idea of learning the components of a kanji before/while learning the kanji comes from [James Heisig's Remember the Kanji](https://www.goodreads.com/book/show/53499726-remembering-the-kanji).)
