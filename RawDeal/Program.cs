﻿using RawDeal;
using RawDealView;

// string folder = "02-InvalidDecks";
// string folder = "04-NoEffects";
// string folder = "05-SuperstarAbilities";
string folder = "06-BasicHybridCards";
// string folder = "07-NoEffectReversals";

// Esta vista permite verificar el comportamiento de un test particular.
// El texto en consola saldrá azúl cuando el output sea el esperado y rojo cuando no lo sea. 
// Cuando aparezca texto rojo el programa entrará en "modo manual"
int idTest = 1;
string pathToTest = Path.Combine("data", $"{folder}-Tests", $"{idTest}.txt");
View view = View.BuildManualTestingView(pathToTest);

// esta vista permite jugar el juego de manera manual
// View view = View.BuildConsoleView();  

string deckFolder = Path.Combine("data", folder);
Game game = new Game(view, deckFolder);
game.Play();