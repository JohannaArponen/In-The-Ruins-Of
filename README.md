
# Anti Retardation Docket

###### DISCLAIMER: These are merely suggestions, if you want to make others' lives harder, you can completely ignore these!

## General

- The earth has almost 8 billion people. Around 25 million are software developers, and many have gone through the problems you have. Luckily alot of this information is shared on the internets and accessible via this thing called Google. Google has a great search engine, typing your question at [google.com](google.com) will most likely give you the whole answer, and at least, some idea of the answer.

- Keep naming conventions consistent. In Unity, we **C**apitalize names, and spaces are allowed, but folders never use spaces

- Usually things are capitalized like `ThisIsALameName` or `A Title of Nothing Cool`.

- Your expressions of emotions and jokes are allowed (but undesirable) when naming things, still, make sure others can understand the name.

## Unity

- Make assets with meaningful names. Most assets will populate the search windows. Having 100 no names or texture #XX's is detrimental when looking for a specific asset.
- If you don't use a unique material for your model, you can simple assing it an already existing one in the import settings of the model. This disables the import of the "no name" material.
![enter image description here](https://i.imgur.com/2MPtrrp.png)

- Put your thing of type X into the folder of type X. If your thing is unique enough and there are more things that have the same uniqueness, make a new folder for those kind of things under the X folder.

- Don't make a folder named X under a folder named X
![]([https://i.imgur.com/JhYeG6C.png](https://i.imgur.com/JhYeG6C.png)) 

- Scenes should probably not consist of one massive prefab named "whole ass level".

- Split large prefabs into smaller prefabs if they are useful and usable alone.

---

- Sort different types of textures/materials to their own folder if necessary. Do not make a new root (Assets) folder.

- Your PSD file is not a PNG file, dont put them in to the same folder. A folder consisting mostly of PNG texture files should not contain random PSD files. Make a folder specifically for source files (under whatever image/texture folder we have (not under the assets folder)).

- If you favorite model appears pink, try reimporting the model (rightclick the model asset and select "reimport"). Additionally you can try: `Edit` **->** `Render Pipeline` **->** `Universal Render Pipeline` **->**`Upgrade Project Materials to UniversalRP Materials`

## Git (sourcetree, github)

- Don't commit changes if you are not sure what they are. Ask a nearby colleague if you should discard or ignore the changes. (Don't make a commit named "ok" and shrug.)

- If there's an error, read it, think about it and try to fix it. Often they tell you what to do.

- Editing something someone else is already editing, e.g. a scene, will likely cause merge conflicts.

## 3DS MAX

- Consider switching to Blender so you don't fall behind the modelling industry.

- Name your materials and meshes in your horrible modelling software, you will need to do this in the future anyways. They are otherwise added to the list of assets with their default name e.g. "no name", cluttering the asset list and workspace.

- Make sure your models pivot is in a logical place (aling your model to the center aka the point at 0, 0, 0).
  ![](https://i.imgur.com/NPSnZvx.png)
  Pivot = thing with arrows. Here it should probably be at the center of the disc.

