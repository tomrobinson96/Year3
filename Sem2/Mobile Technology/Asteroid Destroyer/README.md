### Follow these Instructions to Add a new Page to this App

    Note: Replace "newpage" with the identifier of the new page (e.g.: gallery, chat, profile, etc.)

1. Create a new CONTROLLER for the new page.
    * create the file: **scripts/controllers/newpage.js**
    * create the CONTROLLER CODE, giving it the name of **NewpageController**
```
      app.controller (
      	'NewpageController', [
      		'$scope', function ( $scope )
      		{
            // code here
            // ...
      		}
      	]
      );
```
2. Create a new VIEW for the new page.
    * create the file: **views/newpage.html**
    * create the base VIEW CODE as follows:
```
    <div ng-controller="NewpageController as newpage">
        This is the Newpage Page!
    </div>
```
3. Create a new STYLESHEET for the new page.
    * create the file **stylesheets/newpage.css**
4. Link the new STYLESHEET file to **index.html**
    * add a new **<link>** tag inside **<head>** following the **<link>** for **stylesheets/app.css**
```
    <link rel="stylesheet" href="stylesheets/newpage.css">
```
5. Link the new CONTROLLER file to **index.html**
    * add a new **<script>** tag at the very bottom of the **<body>** tag, but before the closing **</body>** tag
```
    <script src="scripts/controllers/newpage.js"></script>
```
6. Add a new ROUTE in **scripts/app.js** before the **.otherwise()**, and after the last **.when()**
    * must containing the URL link (**/newpage**) 
    * must contain the VIEW link (**views/newpage.html**)
    * code should look like this:
```
    .when (
	    '/newpage', {
		    templateUrl: 'views/newpage.html'
	    }
    )
```
7. Add a new NAVIGATION ITEM in **scripts/controllers/navigation.js** within the **navItems** array after the last item in the list
    * must contain the LINK defined in **scripts/app.js**
    * must contain a sensible TITLE to be displayed in the MENU
    * code should look like this: (MIND THE COMMA, all array elements need to be split apart using a comma)
```
    ,{
        link: "/newpage",
        title: "New Page"
    }
```
8. PROFIT!