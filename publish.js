var ghPages = require("gh-pages");
var fs = require("fs");
var path = require("path");

var resolve = function(dir) { 
    return path.join(__dirname, dir)
};

var packageContents = fs.readFileSync(resolve("package.json"));
var package = JSON.parse(packageContents);
var packageUrl = package.repository.url; 

console.log("Publishing to " + packageUrl); 

ghPages.publish("public", {
    repo: packageUrl
}, function(e) {
    if (e === undefined) {
        console.log("Finished publishing succesfully");
    } else {
        console.log("Error occured while publishing :(", e);
    }
});