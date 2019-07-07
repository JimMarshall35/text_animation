A script to animate text in unity by adding sinusoidal motion to it letter by letter.

Script splits up textmesh pro UI object at runtime into individual letters spaced apart 
-according to a value you specify and childs these letters to the original text object. 
These individual letters are animated by coroutines.

HOW TO USE:
1.) Attatch this script to a textmesh pro text UI object in unity that you want to animate, 
-make sure the transform is centred over the first letter of the word. 

2.) Create a textmesh pro prefab that contains no text, has the same font and size as the text 
-you want to animate and has the transform centred over where the first letter will appear when typed.

3.) create Resources folder in assets and add the prefab, calling it "blank letter".

4.) configure script in the editor so that the text spacing is properly set for the size of your text and 
-change the animation to the way you want it.
