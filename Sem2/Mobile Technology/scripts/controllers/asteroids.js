// textures PNG link
// make sure you first create an account with Imgur.com to add your own
// http://i.imgur.com/UhAqKNg.png // textures
// http://i.imgur.com/A4jyRT0.png // allied star
// http://i.imgur.com/fbkSKxh.png // squid
http: //i.imgur.com/YHpbHxBs.png // speed up ship paw
  // http://i.imgur.com/ui202fS.png?2 //bullet

  app.controller(
    'AsteroidsController', [
      '$scope', '$localStorage',
      function($scope, $localStorage) {

        // code

        // Pixi.JS: https://github.com/kittykatattack/learningPixi
        // Bump.JS: https://github.com/kittykatattack/bump (2D collision detection toolset)

        /********** Configuration **********/
        var
          initialLives = 10, // how many lives does the ship have?
          asteroidMovementSpeed = 5, // pixels per frame
          initialAsteroidCount = 1, // how many asteroids to spawn at the start
          bulletMovementSpeed = 6, // pixels per frame
          bulletsMaxCount = 1, // how many bullets should be active at any one time
          shipMovementSpeed = 5, // pixels per frame
          livesLostPerAsteroidHit = 1, // how many lives are lost when ship is hit by asteroid
          scorePerAsteroidHit = 1, // score per asteroid hit by a bullet
          easingAmount = 10,
          shipMaxSpeed = 20;

        /********** Aliases **********/
        // Aliases
        var
          Container = PIXI.Container,
          autoDetectRenderer = PIXI.autoDetectRenderer,
          loader = PIXI.loader,
          resources = PIXI.loader.resources,
          Sprite = PIXI.Sprite,
          Graphics = PIXI.Graphics,
          Text = PIXI.Text,
          collisionDetection = new Bump(); // collision detection

        var initialContainerWidth, initialContainerHeight;

        /********** OBJECT CONTAINERS **********/
        // objects
        var
          ship,
          score,
          globalScore = 0,
          powerUps,
          lives,
          level = 1,
          scoreText,
          levelText,
          livesText,
          gameNum = 1;
        $scope.playerinput = "";

        // Arrays
        var
          asteroids = [],
          bullets = [];

        // touch object container
        var
          touchStage;

        // helpers
        var
          endGame; // determines whether the current game has ended

        /********** SETUP STAGE **********/
        // Create a Pixi stage and renderer
        var
          stage = new Container(),
          containerElement = angular.element("#asteroids"), // jQuery object
          containerWidth, containerHeight, // actual container height/width
          stageWidth, stageHeight, // stage height/width
          renderer = autoDetectRenderer(0, 0); // don't care about size here

        // attach the drawing board to the View
        containerElement.html(renderer.view);

        // get the actual height and width
        containerWidth = containerElement.innerWidth();
        containerHeight = containerElement.innerHeight();

        // set the stage height and width
        stageWidth = containerWidth;
        stageHeight = containerHeight;

        stage.halfWidth = stageWidth / 2;
        stage.halfHeight = stageHeight / 2;

        // create a container to use when checking stage wall collisions
        var stageContainer = {
          x: 0,
          y: 0,
          height: stageHeight,
          width: stageWidth
        };

        // allow renderer to resize itself as needed
        renderer.autoResize = true;

        // make sure the drawing board has the size we want, width first, then height
        renderer.resize(stageWidth, stageHeight);

        /********** LOAD SPRITES **********/
        if (!PIXI.spritesAlreadyLoaded) {

          // load an image and run the `setup` function when it's done
          loader
            .add({
              name: "asteroid", // reference
              url: "https://i.imgur.com/A4jyRT0.png", // location
              crossOrigin: true // needs this to be able to load images remotely
            }),
            loader
            .add({
              name: "bullet", // reference
              url: "https://i.imgur.com/ui202fS.png?2", // location
              crossOrigin: true // needs this to be able to load images remotely
            }),
            loader
            .add({
              name: "powerUp",
              url: "http://i.imgur.com/YHpbHxBs.png",
              crossOrigin: true
            })
            .load(reloadGame);

          // mark sprites as already loaded
          PIXI.spritesAlreadyLoaded = true;
        } else {



          //setup();
          reloadGame()

        }

        /*****Reloading Levels not working correctly, when reloading scene via this method nothing is displayed on stage
         * leaving it in a playable state with speed increasing after each new scene loaded **********/

        /********** LEVELS OBJECTS SETUP **********/
        // objects
        var
          levels,
          levelsDefinition = {},
          activeLevel;

        /******** Reload Game ****************/
        function reloadGame() {

          //clear stage
          for (var i = stage.children.length - 1; i >= 0; i--) {
            stage.removeChild(stage.children[i]);
          }
          // Set up new stage
          stageSetup();
          //Set up new ship game objects
          setup()

          // setup the levels object for the current game instance
          //levelsSetup();

          // run the setup
          //gameSetup();

        }

        /********** GAME CLEANUP **********/
        function cleanup() {

          // end the game loop
          endGame = true;

          // if there is a stage, kill everything within it
          if (stage) {

            // destroy the stage and its children, but keep the associated textures
            stage.destroy({
              children: true,
              texture: false,
              baseTexture: false
            });

          }

        }

        /********** STAGE RESETUP **********/
        function stageSetup() {

          // create the stage container
          stage = new Container();

          // store the half height/width of the stage within itself for easy access
          stage.halfWidth = stageWidth / 2;
          stage.halfHeight = stageHeight / 2;

        }

        /********** LEVELS SETUP **********/
        function levelsSetup() {

          // do a deep copy of the above levelsDefinition object, thus keeping the levels definition always intact
          levels = {
            level_1: new levelsDefinition.level_1()
          };

        }

        /********** GAME SETUP **********/
        function gameSetup() {

          setup()

          // starting a new game
          endGame = false;

          // Start the game loop
          //gameLoop();

        }

        /********** SWITCH LEVEL FUNCTION (params = (newLevel, forceSetup: boolean)) **********/
        // based on information found at http://plnkr.co/edit/r9U0uNCjeu1e4UudEZit?p=info, Dinita Razvan
        function switchLevel(newLevel, forceSetup) {

          // if the requested level doesn't exist, then stop and show the issue in console
          if (!levels[newLevel]) {
            console.error("switchLevel: Could not switch to <" + newLevel + "> because it doesn't exist! You've perhaps mistyped its name?");
            return;
          }

          // if there is a stage there and it's visible
          if (activeLevel && activeLevel.stage && activeLevel.stage.visible) {

            // hide the current one
            activeLevel.stage.visible = false;

          }

          // make the switch
          activeLevel = levels[newLevel];

          // is a setup forcefully requested? or perhaps the level needs setting up
          if (forceSetup || !activeLevel.hasBeenSetUp) {
            activeLevel.setup();
          }

          // make it visible
          activeLevel.stage.visible = true;

        }

        /********** GAME SETUP **********/
        function setup() {

          // setup the score text
          setupScore();

          // setup the lives text
          setupLives();

          // setup the level text
          setupLevelText();

          // setup the powerups
          setupPowerUps();

          // setup the ship
          setupShip();

          // setup the bullets
          setupBullets();

          // setup the asteroids
          setupAsteroid();

          //setupPlayerName();

          // setup the touch support
          setupTouch();

          // set the initial game state
          state = play;

          // starting a new game
          endGame = false;



          // Start the game loop
          gameLoop();

        }

        /********** ASTEROID SETUP: CONSTRUCTOR **********/
        function Asteroid(x, y, behaviour, isCopy) {

          // Create the asteroid sprite
          var asteroid = new PIXI.Sprite(resources["asteroid"].texture);

          // set its position
          asteroid.position.set(x, y);

          // set the velocity
          asteroid.vx = asteroidMovementSpeed;
          asteroid.vy = asteroidMovementSpeed;

          // if it's a copy, give it a negative velocity
          if (isCopy) {

            asteroid.vx *= -1;
            asteroid.vy *= -1;

          }

          // set the anchor point to its centre
          //asteroid.anchor.set(0.50, 0.50);

          // set its behaviour for when it hits something
          asteroid.behaviour = behaviour;

          // add the asteroid to stage
          stage.addChild(asteroid);



          // return it for later use
          return asteroid;

        }

        /********** ASTEROID SETUP: SPAWNING THEM**********/
        function setupAsteroid(x, y) {

          // by default this is a copy asteroid (after collision)
          var isCopy = true;



          // if there are no x and y values defined (initial spawn)
          if (!x || !y) {

            // generate some random X and Y position values
            x = randomNumberBetween(stageWidth / 3, stageWidth / 3 * 2),
              y = randomNumberBetween(stageHeight / 4, stageHeight / 3);

            // not a copy
            isCopy = false;

          }

          // behaviour for when it gets hit
          var behaviour = function() {

            // mark it for removal if score > 8
            if (score > 8) {
              this.hasBeenHit = true;
              if (hasBeenHit) {
                isCopy = true;
              }

            }

            // if this is a copy, stop doing anything else
            if (isCopy) {

              return;

            }
            // otherwise, it's the original, so get ready to spawn to more

            // calculate the new position
            var
            // position for the first new asteroid
              x1, y1,
              // position for the second new asteroid
              x2, y2;


            // determine x positions of the two new asteroids
            if (this.x + this.width >= stageWidth) {
              x1 = stageWidth - this.x - this.width * 2;
              x2 = stageWidth - this.x - this.width;
            } else if (this.x - this.width <= 0) {
              x1 = this.width * 2;
              x2 = this.width;
            } else {
              x1 = this.x - this.width * 2;
              x2 = this.x - this.width;
            }

            // determine y positions of the two new asteroids
            if (this.y + this.height >= stageHeight) {
              y1 = stageHeight - this.y - this.height * 2;
              y2 = stageHeight - this.y - this.height;
            } else if (this.y - this.height <= 0) {
              y1 = this.height * 2;
              y2 = this.height;
            } else {
              y1 = this.y - this.height * 2;
              y2 = this.y - this.height;
            }

            //spawn one copy at the above determined position and one original star 
            //(which will have opposite velocity to "copy") so the game continues until score is reached
            if (score < 6) {
              setupAsteroid(x1, y1);
            }
          }

          asteroids.push(Asteroid(x, y, behaviour, isCopy));

        }

        /********** SHIP SETUP **********/
        // based on information found at http://plnkr.co/edit/r9U0uNCjeu1e4UudEZit?p=info, Dinita Razvan
        function setupShip() {

          // create a new shape that will eventually look like a ship
          ship = new Graphics();

          var
            shipHeight = stageHeight / 35, // 1/30th of the stage's height
            shipWidth = stageWidth / 15; // 1/30th of the stage's width

          // set the line style (width, colour, alpha/transparency) 
          // colour:
          //    0xRRGGBB (red, green, blue) (Hex numbers)
          //        => each R,G,B character can have the following values: 0 .. 9 and a .. f
          //        => the higher the value, the lighter the primary colours that will be used to form this colour mix
          //        => value order in order of intensity: 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, a, b, c, d, e, f
          ship
            .lineStyle(1, 0xFF3300, 1) // line width, colour, and alpha/transparency
            .beginFill(0x66CCFF) // ship colour
            .drawRect(0, 0, shipWidth, shipHeight) // draw the actual ship (use 0, 0 as the first 2 values, the final position will be set below)
            .endFill(); // end the drawing process for this ship

          // set the ship position
          // relevant to the left side of the stage, set it at 1/3rd of the stage width 
          // (so it's centered since the paddleWidth is itself 1/3rd of the stage width)
          ship.x = stageWidth / 2;
          // relative to the top side of the stage, set it at the bottom of the stage, minus half of the 
          ship.y = stageHeight - shipHeight - 20; // take 2 pixels away to account for the drawing line's width

          // set the velocity
          ship.vx = shipMovementSpeed;
          ship.vy = shipMovementSpeed;

          // add the ship to the stage
          stage.addChild(ship);

        }

        /********** BULLET SETUP: CONSTRUCTOR **********/
        function Bullet(behaviour, eX, eY) {

          // Create the star sprite
          var bullet = new Sprite(resources["bullet"].texture);

          // set its position
          bullet.position.set(ship.x + ship.width, ship.y);

          // scale it to 1/20th of its original size
          bullet.scale.set(0.05, 0.05);

          this.eX = eX;
          this.eY = eY;

          // set the anchor point to its centre
          //bullet.anchor.set(0.50, 0.50);

          // set the velocity
          bullet.vx = bulletMovementSpeed;
          bullet.vy = bulletMovementSpeed;

          // set its behaviour
          bullet.behaviour = behaviour;

          // add the bullet to the stage
          stage.addChild(bullet);

          // return it for later use
          return bullet;

        }


        /********** SETUP BULLETS **********/
        function setupBullets() {

          // setup the bullet behaviour for when it hits something
          var behaviour = function() {

            // mark it for removal
            this.hasBeenHit = true;

            // update the score
            updateScore("bulletHitAsteroid");
            updateScore("bulletHitAsteroidGlobal")

            // just setup another one
            setupBullets();

          }

          // check how many we need to spawn
          // potentially none if there are already max bullets out
          var count = bulletsMaxCount - bullets.length;

          for (var i = 0; i < count; i++) {

            bullets.push(Bullet(behaviour));

          }

        }

        /********** SETUP POWER UPS **********/
        function setupPowerUps() {

          // reset the powerUps container to an empty array
          powerUps = [];

        }

        /********** POWER UP - CONSTRUCTOR **********/
        // x, y - starting coordinates
        // behaviour:
        //    function() { /* CODE */ }
        function PowerUp(x, y, behaviour) {

          // Create the star sprite
          var powerUp = new Sprite(resources["powerUp"].texture);

          // change its scale
          powerUp.scale.set(0.50, 0.50);

          // set its position
          powerUp.position.set(x, y);

          // set anchor to middle of object
          powerUp.anchor.set(0.5, 0.5);

          // add custom behaviour
          powerUp.behaviour = behaviour;

          // add the star to
          stage.addChild(powerUp);

          // return the object for later use
          return powerUp;

        }

        /********** SPAWN A STAR SPEED-UP POWER-UP **********/
        function spawnSpeedUpPowerUp() {

          // define coordinates for this power-up
          var
            x = Math.floor((Math.random() * stageWidth) + 1),
            y = Math.floor((Math.random() * stageHeight) + 1),
            behaviour = function() {

              // this refers to the power up
              // move it outside the stage area
              this.position.set(-100, -100);

              // also make it invisible
              this.visible = false;


              ship.vx++;
              ship.vy++;


            };


          powerUps.push(PowerUp(x, y, behaviour));

        }

        /********** TOUCH SETUP **********/
        function setupTouch() {

          touchStage = touch(renderer.view, stage, "anywhere");

        }

        /********** SCORE TEXT SETUP **********/
        function setupScore() {

          // reset the score variable
          score = 0;

          // create the score text object
          scoreText = new Text(
            "Score: " + score, {
              fontFamily: "Arial",
              fontSize: 14,
              fill: "white"
            }
          );

          // set the text position
          scoreText.position.set(10, 10); // top left corner

          // add it to the stage
          stage.addChild(scoreText);
        }

        /********** Level TEXT SETUP **********/
        function setupLevelText() {


          // create the score text object
          LevelText = new Text(
            "Level: " + level, {
              fontFamily: "Arial",
              fontSize: 14,
              fill: "white"
            }
          );

          // set the text position
          LevelText.position.set(stageWidth / 2, 10); // middle top

          // add it to the stage
          stage.addChild(LevelText);
        }

        /********** End Level TEXT SETUP **********/
        function endLevelText() {


          // create the score text object
          EndLevelText = new Text(
            "Game Over ", {
              fontFamily: "Arial",
              fontSize: 40,
              fill: "white"
            }
          );

          // create the score text object
          EndLevelLevel = new Text(
            "Level: " + level, {
              fontFamily: "Arial",
              fontSize: 20,
              fill: "white"
            }
          );

          // create the score text object
          EndLevelScore = new Text(
            "Score: " + globalScore, {
              fontFamily: "Arial",
              fontSize: 20,
              fill: "white"
            }
          );

          // set the text position
          EndLevelText.position.set(stageWidth / 2.5, stageHeight / 3); // Middle

          // set the text position
          EndLevelLevel.position.set(stageWidth / 3.5, 250); // Middle Left

          // set the text position
          EndLevelScore.position.set(stageWidth / 1.5, 250); // Middle Right

          // add it to the stage
          stage.addChild(EndLevelLevel);

          // add it to the stage
          stage.addChild(EndLevelText);

          // add it to the stage
          stage.addChild(EndLevelScore);
        }

        /********** LIVES TEXT SETUP **********/
        function setupLives() {

          // reset the lives variable
          lives = initialLives;

          // create the lives text object
          livesText = new Text(
            "Lives: " + lives, {
              fontFamily: "Arial",
              fontSize: 14,
              fill: "white"
            }
          );

          // set the text position
          livesText.position.set(stageWidth - 80, 10);; // top right corner

          // add it to the stage
          stage.addChild(livesText);

        }

        /********** GAME PLAY LOGIC **********/
        function play() {

          // handle the paddle movement
          handleShipMovement();

          // handle the star movement
          handleAsteroidsMovement();

          // handle the bullet movement
          handleBulletsMovement();

          handleShipCollisionWithPowerUps();

          // cleanup
          cleanupHitSprites();

        }

        /********** GAME PLAY LOGIC **********/
        function pause() {

          // cleanup
          cleanupHitSprites();

        }

        /********** CLEANUP SPRITES **********/
        // based on information found at http://plnkr.co/edit/r9U0uNCjeu1e4UudEZit?p=info, Dinita Ravzan
        function cleanupHitSprites() {

          // define groups
          var groupsOfSpritesToCheck = [{
            sprites: bullets,
            ifEmpty: function() { // do this if this group is empty

              // create more bullets
              setupBullets();

            }
          }, {
            sprites: asteroids,
            ifEmpty: function() { // do this if this group is empty

              // create another asteroid
              setupAsteroid();

            }
          }];

          // do the cleanup on the above groups
          cleanupSprites(groupsOfSpritesToCheck);

        }

        /********** SHIP MOVEMENT **********/
        // based on information found at http://plnkr.co/edit/r9U0uNCjeu1e4UudEZit?p=info, Dinita Razvan
        function handleShipMovement() {
          // get the new touch position, if relevant
          // if player is touching the stage at this time
          if (touchStage.isDown) {

            // define/update the new moveTo position
            ship.moveTo = touchStage.position;

          }

          if (score > 10 && asteroids.length == 1) {

            return;
          }



          var
          // test collision between the ship and the stage walls
            collision = collisionDetection.contain(ship, stageContainer);

          // if there is no collision and there is a moveTo set of coordinates defined
          if (!collision && ship.moveTo) {

            // get the ship's touch position
            var
              moveFrom = ship.position;

            // if there's still room to move according to the character's movement speed
            if (Math.abs(moveFrom.x - ship.moveTo.x) > ship.vx) {

              // if the character's X position is greater than the ship's touch's X position
              if (moveFrom.x > ship.moveTo.x) {

                // then decrease the character's X position to bring it closer to the ship's touch's X position
                ship.x -= ship.vx;

                // otherwise if the character's X position is less than the ship's touch's X position
              } else if (moveFrom.x < ship.moveTo.x) {

                // then increase the character's X position to bring it closer to the ship's touch's X position
                ship.x += ship.vx;

              }

            }



            // if there's still room to move according to the character's movement speed
            if (Math.abs(moveFrom.y - ship.moveTo.y) > ship.vy) {

              // if the character's Y position is greater than the ship's touch's Y position
              if (moveFrom.y > ship.moveTo.y) {

                // then decrease the character's Y position to bring it closer to the ship's touch's Y position
                ship.y -= ship.vy;

                // otherwise if the character's Y position is less than the ship's touch's Y position
              } else if (moveFrom.y < ship.moveTo.y) {

                // then increase the character's Y position to bring it closer to the ship's touch's Y position
                ship.y += ship.vy;

              }

            }

          }


        }

        /********** ASTEROIDS MOVEMENT **********/
        // based on information found at http://plnkr.co/edit/r9U0uNCjeu1e4UudEZit?p=info, Dinita Ravzan
        function handleAsteroidsMovement() {

          for (var i = 0; i < asteroids.length; i++) {

            var asteroid = asteroids[i];

            asteroid.x += asteroid.vx;
            asteroid.y += asteroid.vy;

            // check whether the asteroid has hit the stage walls and handle it accordingly
            handleAsteroidCollisionWithWalls(asteroid);

            // check whether the asteroid has hit the ship and handle it accordingly
            handleAsteroidCollisionWithShip(asteroid);

            // check whether the asteroid has hit a bullet and handle it accordingly
            handleAsteroidCollisionWithBullets(asteroid);


            // if player is in level 2 or higher and there score reaches 1 then remove the text telling them they have completed a level
            if (score >= 1 && level > 1) {
              stage.removeChild(winLevelText)
            }

            if (score > 10 && asteroids.length == 1) {

              asteroid.vx = 0,
                asteroid.vy = 0;

              // create the level win text object
              winLevelText = new Text(
                "LEVEL COMPLETE         Score: " + globalScore, {
                  fontFamily: "Arial",
                  fontSize: 30,
                  fill: "white"
                }
              );

              // set the text position
              winLevelText.position.set(stageWidth / 4, stageHeight / 2); // middle

              // add it to the stage
              stage.addChild(winLevelText);

              level++;

              setTimeout(spawnNewLevel(), 20000);

            }

          }

        }

        //remove all stage objects and then spawn new ones
        function spawnNewLevel() {
          scoreText.destroy(),
            LevelText.destroy(),
            livesText.destroy(),
            ship.destroy(),
            //setInterval(winLevelText.destroy, 50000);

            //bullets.destroy();
            setTimeout(setup(), 50000);

        }

        /********** BULLETS MOVEMENT **********/
        function handleBulletsMovement() {

          for (var i = 0; i < bullets.length; i++) {

            var bullet = bullets[i];

            bullet.x += bullet.vx;

            // if this bullet is at the edge of the stage
            if (bullet.x >= stageWidth) {

              // mark it for removal
              bullet.hasBeenHit = true;

              // setup new ones as required
              setupBullets();

            }

          }

          if (score > 10 && asteroids.length == 1) {

            bullet.vx = 0,
              bullet.vy = 0;
          }

        }

        /********** STAR COLLISION WITH POWER-UPS **********/
        function handleShipCollisionWithPowerUps() {

          // check whether there are any visible power-ups
          var powerUpsExist = false;
          for (var i = 0; i < powerUps.length; i++) {
            if (powerUps[i].visible) {
              powerUpsExist = true;
              break;
            }
          }

          // if there aren't any, spawn one
          if (!powerUpsExist) {
            spawnSpeedUpPowerUp();
          }

          var
            reactToHit = true,
            bounceOnHit = false,
            useGlobalCoordinates = false,
            shipVsPowerUps = collisionDetection.hit(
              ship,
              powerUps,
              reactToHit, bounceOnHit, useGlobalCoordinates,
              function(collision, powerUp) {

                // `collision` tells you the side on the star that the collision occurred on.
                // `powerUp` is the sprite from the `powerUps` array
                // that the star is colliding with

                // run the powerUp behaviour
                powerUp.behaviour();

              }
            );

        }

        /********** ASTEROID COLLISION WITH STAGE WALLS **********/
        function handleAsteroidCollisionWithWalls(asteroid) {

          var
          // take into account the asteroid dimensions so there's a smooth transition
            collision = collisionDetection.contain(asteroid, {
            x: asteroid.width * -1,
            y: asteroid.height * -1,
            width: stageWidth + asteroid.width,
            height: stageHeight + asteroid.height
          }); // the third "true" parameter tells the asteroid to automatically bounce off the stage walls

          // also, if there is a collision with the walls, then decide what to do about it
          if (collision) {

            // if left margin was hit
            if (collision.has("left")) {

              // move the asteroid to the right side on the x axis
              asteroid.x = stageWidth;

            }

            // if right margin was hit
            if (collision.has("right")) {

              // move the asteroid to the left side on the x axis
              asteroid.x = asteroid.width * -1;

            }

            // if top margin was hit
            if (collision.has("top")) {

              // move the asteroid to the bottom side on the y axis
              asteroid.y = stageHeight;

            }

            // if bottom margin was hit
            if (collision.has("bottom")) {

              // move the asteroid to the top side on the y axis
              asteroid.y = asteroid.height * -1;

            }

          }

        }

        /********** ASTEROID COLLISION WITH SHIP **********/
        function handleAsteroidCollisionWithShip(asteroid) {

          // third parameter "true" makes the asteroid bounce off automatically if it hits the ship
          var
            collision = collisionDetection.rectangleCollision(asteroid, ship, true);

          // if the asteroid has collided with the ship, then decide what to do
          if (collision) {

            // update the score
            updateScore("asteroidHitShip");


          }

        }

        /********** ASTEROID COLLISION WITH BULLETS **********/
        function handleAsteroidCollisionWithBullets(asteroid) {

          var
            anyHitMade = false;
          reactToHit = true,
            bounceOnHit = true,
            useGlobalCoordinates = false,
            asteroidVsBullets = collisionDetection.hit(
              asteroid,
              bullets,
              reactToHit, bounceOnHit, useGlobalCoordinates,
              function(collision, bullet) {

                // stop reacting to collisions, a bullet has hit the asteroid
                if (anyHitMade) {
                  return;
                }

                // `collision` tells you the side on the asteroid that the collision occurred on.
                // `bullet` is the sprite from the `bullets` array
                // that the asteroid is colliding with

                console.log("hit");

                // run the behaviour
                bullet.behaviour();

                // also trigger the asteroid behaviour
                asteroid.behaviour();

                // disable any further collision behaviour
                anyHitMade = true;

              }
            );

        }

        /************ Display Name & Score **********/
        // When player inserts name and clicks submit, score and name will be sent to leaderboard
        $scope.DisplayName = function(name) {

          var nameObject = {
            score: globalScore,
            playerName: $scope.playerinput
          };

          // check if the scores array exists within localstorage
          if (!$localStorage.scoresFull) {
            $localStorage.scoresFull = [];
          }

          // record the score in localstorage
          // add name object to the end of the array
          $localStorage.scoresFull.push(nameObject);

          // sort scores highest to lowest
          $localStorage.scoresFull.sort(function(scoreA, scoreB) {
            return scoreA.score < scoreB.score;
          });

          // while more than 3 scores
          while ($localStorage.scoresFull.length > 3) {
            // remove last score of the array
            $localStorage.scoresFull.pop();
          }

        }


        /********** GAME OVER **********/
        function gameOver() {

          state = pause;

          /*//clear stage
          for (var i = stage.children.length - 1; i >= 0; i--) {
            stage.removeChild(stage.children[i]);
          } */

          //show end level text
          endLevelText();




        }

        /********** GAME WON **********/
        function winGame() {

          // inform the user
          alert("You've won!");

          // end the game
          endGame = true;

        }

        /********** SCORE UPDATE **********/
        function updateScore(reason) {

          // if no reason given, assume the default reason for updating the score
          // is a paddle hit
          if (!reason) {
            type = "bulletHitAsteroid";
          }

          // add more cases here as required
          switch (reason) {

            case "bulletHitAsteroid":

              // update score
              score += scorePerAsteroidHit;

              break;
            case "bulletHitAsteroidGlobal":

              // update score
              globalScore += scorePerAsteroidHit;
              console.log(globalScore)

              break;
            case "asteroidHitShip":

              // update Lives
              lives -= livesLostPerAsteroidHit;

              if (lives <= 0) {

                gameOver();

              }

              break;
          }

          // update the text
          scoreText.text = "Score: " + score;
          livesText.text = "Lives: " + lives;

        }



        /********** GAME LOOP **********/
        function gameLoop() {

          // if the game has ended, stop the game loop from running again
          if (endGame) {
            return;
          }

          // Loop this function at 60 frames per second
          requestAnimationFrame(gameLoop);

          // run whatever the current game state is
          state();

          // Render the stage to see the updated output
          renderer.render(stage);

        }

        /*************Resize stage working but the objects within the stage are not being shown********************/

        /********** STAGE RESIZE (param = "small" OR "full") **********/
        // based on information found at http://plnkr.co/edit/r9U0uNCjeu1e4UudEZit?p=info, Dinita Ravzan
        function resizeStage(which) {

          var
            relativeRatio, // used to determine the stage size relative to the stage's parent container
            ratio, // used to determine the stage's internal ratio for scaling purposes
            windowElem, // jQuery element representing the browser window
            buttons = angular.element("#screenButtons"); // jQuery element representing the screen size buttons

          switch (which) {

            case "small":

              // get a handle on the window
              windowElem = angular.element(window);

              // get the actual height and width of the stage container
              /*containerWidth = initialContainerWidth;
              containerHeight = initialContainerHeight*/

              // define the required ratio
              relativeRatio = {
                height: 1 / 2, // 50% of the available height
                width: 1 // 100% of the available width
              };

              // remove the fullscreen class from the HTMl elements
              // these classes are defined in the file "stylesheets/asteroids.css"
              containerElement.removeClass("fullScreen");
              buttons.removeClass("buttons-fullScreen");
              state = pause;
              spawnNewLevel();
              state = play;

              break;

            case "full":

              // get a handle on the window
              windowElem = angular.element(window);

              // get the actual height and width of the window
              containerWidth = windowElem.innerWidth();
              containerHeight = windowElem.innerHeight();

              // make the HTML elements fullscreen as well
              // these classes are defined in the file "stylesheets/asteroids.css"
              containerElement.addClass("fullScreen");
              buttons.addClass("buttons-fullScreen");

              // define the required relative ratio
              relativeRatio = {
                height: 1, // 100% of the available height
                width: 1 // 100% of the available width
              };
              state = pause;
              spawnNewLevel();
              state = play;

              break;

          }

          // Determine which screen dimension is most constrained
          ratio = Math.max(containerWidth / initialContainerWidth, containerHeight / initialContainerHeight);

          // Scale the view appropriately to fill that dimension
          stage.scale.x = stage.scale.y = ratio;

          // set the stage height and width, taking into account the required relative ratio
          stageWidth = containerWidth * relativeRatio.width;
          stageHeight = containerHeight * relativeRatio.height;

          // create a container to use when checking stage wall collisions
          stageContainer = {
            x: 0,
            y: 0,
            height: stageHeight,
            width: stageWidth
          };

          // make sure the drawing board has the size we want, width first, then height
          renderer.resize(stageWidth, stageHeight);

        }

        /********** $SCOPE FULLSCREEN FUNCTION **********/
        $scope.MakeFullScreen = function() {

          resizeStage("full");
          $scope.isFullScreen = true;

        };

        /********** $SCOPE SMALLSCREEN FUNCTION **********/
        $scope.MakeSmallScreen = function() {

          resizeStage("small");
          $scope.isFullScreen = false;


        };

        /********** SUPPORT: KEYBOARD **********/
        // Keyboard support
        // https://github.com/kittykatattack/learningPixi#keyboard
        // kayboard.isDown and keyboard.isUp can be used to check the state of a key
        function keyboard(keyCode) {

          var key = {};

          key.code = keyCode;
          key.isDown = false;
          key.isUp = true;
          key.press = undefined;
          key.release = undefined;

          //The `downHandler`
          key.downHandler = function(event) {
            if (event.keyCode === key.code) {
              if (key.isUp && key.press) key.press();
              key.isDown = true;
              key.isUp = false;
            }
            event.preventDefault();
          };

          //The `upHandler`
          key.upHandler = function(event) {
            if (event.keyCode === key.code) {
              if (key.isDown && key.release) key.release();
              key.isDown = false;
              key.isUp = true;
            }
            event.preventDefault();
          };

          //Attach event listeners
          window.addEventListener(
            "keydown", key.downHandler.bind(key), false
          );
          window.addEventListener(
            "keyup", key.upHandler.bind(key), false
          );

          return key;

        }

        /********** SUPPORT: TOUCH **********/
        // based on information found at http://plnkr.co/edit/r9U0uNCjeu1e4UudEZit?p=info, Dinita Razvan
        // returns:
        //    touchObject:
        //      isDown: boolean
        //      isUp: boolean
        //      position: // user's touch position within the stage
        //        x: int
        //        y: int
        touch = function(canvas, pixiStage, part) {

          /**
           *  0 - x,y point indicating the top left corner of each area
           * 
           *                  halfWidth       halfWidth
           *              0 ------------ 0 --------------
           *              |              |               |
           *  halfHeight  |  topLeft     |  topRight     |   halfHeight
           *              |              |               |
           *              0 ------------ 0 --------------|
           *              |              |               |
           *  halfHeight  |  bottomLeft  |  bottomRight  |   halfHeight
           *              |              |               |
           *               ------------------------------
           *                  halfWidth       halfWidth
           */

          // split the stage into 4 areas
          // these will be used by the Bump library to determine collision with the point
          var stageParts = {
            "topLeft": {
              x: 0,
              y: 0, // area top left position
              width: pixiStage.halfWidth,
              height: pixiStage.halfHeight // area height and width
            },
            "topRight": {
              x: pixiStage.halfWidth,
              y: 0, // area top left position
              width: pixiStage.halfWidth,
              height: pixiStage.halfHeight // area height and width
            },
            "bottomLeft": {
              x: 0,
              y: pixiStage.halfHeight, // area top left position
              width: pixiStage.halfWidth,
              height: pixiStage.halfHeight // area height and width
            },
            "bottomRight": {
              x: pixiStage.halfWidth,
              y: pixiStage.halfHeight, // area top left position
              width: pixiStage.halfWidth,
              height: pixiStage.halfHeight // area height and width
            },
            "anywhere": {
              x: 0,
              y: 0, // area top left position
              width: pixiStage.halfWidth * 2,
              height: pixiStage.halfHeight * 2 // area height and width
            }
          };

          // if the given part is not defined above, throw an error in the console
          if (!stageParts[part]) {
            throw new Error("Stage part ('" + part + "') not recognised!");
          }

          // object that will be returned from this function, similarly to how the keyboard support works
          var touchObject = {
            part: part,
            isDown: false,
            isUp: false
          };

          // See here for more information on jQuery.on() : http://api.jquery.com/on/
          angular.element(canvas).on("touchstart mousedown pointerdown", function(event) {

            // get the point coordinates of the user's touch
            var point = getEventPos(this, event);

            // if the user touched within the configured part, then set it accordingly
            if (collisionDetection.hitTestPoint(point, stageParts[part])) {

              // mark the current touch object as being pressed / down
              touchObject.isDown = true;
              touchObject.isUp = false;

              // point: { x: number, y: number }
              touchObject.position = point;

            }

          });

          // See here for more information on jQuery.on() : http://api.jquery.com/on/
          angular.element(canvas).on("touchend mouseup pointerup", function(event) {

            // get the point coordinates of the user's touch
            var point = getEventPos(this, event);

            // if the user touched within the configured part, then set it accordingly
            if (collisionDetection.hitTestPoint(point, stageParts[part])) {

              // mark the current touch object as NOt being pressed / down
              touchObject.isUp = true;
              touchObject.isDown = false;

            }

          });

          // supporting function
          // Based on code found at: https://bencentra.com/code/2014/12/05/html5-canvas-touch-events.html
          function getEventPos(canvasEl, event) {

            // get the canvas element bounding coordinates
            var rect = canvasEl.getBoundingClientRect();

            // return an { x, y } object with the user's event coordinates 
            return {
              x: event.clientX - rect.left,
              y: event.clientY - rect.top
            };

          }

          return touchObject;

        }

        /********** SUPPORT: RANDOM NUMBER BETWEEN **********/
        // based on information found at http://plnkr.co/edit/r9U0uNCjeu1e4UudEZit?p=info, Dinita Razvan
        function randomNumberBetween(min, max) {

          return Math.floor(Math.random() * (max - min + 1) + min);

        }

        /********** SUPPORT: CLEANUP SPRITES **********/
        // based on information found at http://plnkr.co/edit/r9U0uNCjeu1e4UudEZit?p=info, Dinita Razvan
        function cleanupSprites(groupsOfSpritesToCheck) {

          // don't change this unless you know exactly why you are doing it
          // just add/remove groups to the above

          var
          // will be used below to detect when objects have been removed from a group
            removed,
            // will be used below to store a reference to the sprites & their group (defined above) that are currently being checked
            group, sprites,
            // will be used below to store a reference to the object that is currently being checked
            sprite;

          for (var groupIndex = 0; groupIndex < groupsOfSpritesToCheck.length; groupIndex++) {

            group = groupsOfSpritesToCheck[groupIndex];
            sprites = group.sprites;

            for (var spriteIndex = sprites.length - 1; spriteIndex >= 0; spriteIndex--) {

              // grab a reference to the object
              sprite = sprites[spriteIndex];

              // object hit!
              if (sprite.hasBeenHit) {

                // destroy the object
                sprite.destroy();

                // remove the object from the group
                sprites.splice(spriteIndex, 1);

                // stop the current for loop
                break;

              }

            }

            // if there aren't any other objects left and there is some behaviour for this
            if (!sprites.length && group.ifEmpty && typeof group.ifEmpty === "function") {

              // run the empty behaviour defined above
              group.ifEmpty();

            }

          }

        }

      }
    ]
  );