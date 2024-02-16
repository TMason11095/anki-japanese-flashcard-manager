# Anki Japanese Flashcard Manager
C# project to take an Anki collection.anki2 database file and rearrange Japanese decks based on what cards you're currently learning and have already learned.

## ankiBindingTags.json
JSON file used to link Anki Tags from the .anki2 file to the program.
* deckTag - Used to identify the deck name from the deck's description. (ex. "deck:")  
![image](https://github.com/TMason11095/anki-japanese-flashcard-manager/assets/134988352/48edb903-1fdb-439c-a71c-de714f4b7380)
* resourceDecks - Used to map resource decks using the deckTag.
  * kanji - Name of the deck that holds kanji cards that may be components of kanji you're currently learning. (ex. "KanjiResource")
    * Example use of deck: If the kanji 飲 (drink) is in your newDeck and the kanji 食 (food) and 欠 (lack) are in the resourceDeck, then the program will move the resource cards into your newDeck. (The idea of learning the components of a kanji before/while learning the kanji comes from [James Heisig's Remember the Kanji](https://www.goodreads.com/book/show/53499726-remembering-the-kanji).)
* newDecks - Used to map newDecks (in-between deck for the program to check and see if any additional cards need to be moved from resourceDeck along with it.) using the deckTag.
  * kanji - Name of the deck that holds new kanji that you've just started learning or want to learn. (ex. "NewKanji")
* learningDecks - Used to map decks using the deckTag that you're learning from.
  * kanji - Name of the deck that holds kanji that you're learning. (ex. "LearningDecks")
* noteTags - Used to identify the type or use of a note from their tags.  
![image](https://github.com/TMason11095/anki-japanese-flashcard-manager/assets/134988352/757aeba4-2fba-4660-8d9e-1e48446a790f)
  * kanjiId - Name of the prefix used to find the kanji's id. (ex. "kid:")
  * subKanjiId - Name of the prefix used to find the kanji's sub kanji id. (ex. "skid:")
* noteIntervalLimits - A note's minimum number of interval days before the app processes it.
  * moveFromNewKanji - Minimum interval needed for a kanji note in the newKanji deck to be moved into the learningKanji deck. (ex. 7)
