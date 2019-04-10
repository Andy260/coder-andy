/// <binding BeforeBuild='min, min:css, min:js, lib' Clean='clean, clean:js, clean:css' />
"use strict";

const gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

const paths = {
    webroot: "./wwwroot/"
};

// Paths
paths.js            = paths.webroot + "js/**/*.js";
paths.minJs         = paths.webroot + "js/**/*.min.js";
paths.css           = paths.webroot + "css/**/*.css";
paths.minCss        = paths.webroot + "css/**/*.min.css";
paths.concatJsDest  = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";
paths.nodeRoot      = "./node_modules/";
paths.nodeDest      = paths.webroot + "lib/";

// Clean tasks
gulp.task("clean:js", done => rimraf(paths.concatJsDest, done));
gulp.task("clean:css", done => rimraf(paths.concatCssDest, done));
gulp.task("clean:lib", done => rimraf(paths.nodeDest, done));
gulp.task("clean", gulp.series(["clean:js", "clean:css", "clean:lib"]));

// Minify Javascript files
gulp.task("min:js", () => {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});
// Minify CSS files
gulp.task("min:css", () => {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});
// Minify Tasks
gulp.task("min", gulp.series(["min:js", "min:css"]));

// Copy Bootstrap distribution files to wwwroot
gulp.task("lib:bootstrap", function () {
    gulp.src(paths.nodeRoot + "bootstrap/dist/js/*")
        .pipe(gulp.dest(paths.nodeDest + "/bootstrap/dist/js"));

    return gulp.src(paths.nodeRoot + "bootstrap/dist/css/*")
        .pipe(gulp.dest(paths.nodeDest + "/bootstrap/dist/css"));
});
// Copy jQuery distribution files to wwwroot 
gulp.task("lib:jquery", function () {
    return gulp.src(paths.nodeRoot + "jquery/dist/*")
        .pipe(gulp.dest(paths.nodeDest + "jquery/dist"));
});
// Copy jQuery Validation Plugin distribution files to wwwroot
gulp.task("lib:jquery-validation", function () {
    return gulp.src(paths.nodeRoot + "jquery-validation/dist/*")
        .pipe(gulp.dest(paths.nodeDest + "jquery-validation/dist"));
});
// Copy jQuery Unobtrusive Validation distribution files to wwwroot
gulp.task("lib:jquery-validation-unobtrusive", function () {
    return gulp.src(paths.nodeRoot + "jquery-validation-unobtrusive/dist/*")
        .pipe(gulp.dest(paths.nodeDest + "jquery-validation-unobtrusive/dist"));
});
// Copy Font Awesome files to wwwroot
gulp.task("lib:fontawesome-free", function () {
    gulp.src(paths.nodeRoot + "@fortawesome/fontawesome-free/css/*")
        .pipe(gulp.dest(paths.nodeDest + "fontawesome-free/css"));

    gulp.src(paths.nodeRoot + "@fortawesome/fontawesome-free/js/*")
        .pipe(gulp.dest(paths.nodeDest + "fontawesome-free/js"));

    gulp.src(paths.nodeRoot + "@fortawesome/fontawesome-free/sprites/*")
        .pipe(gulp.dest(paths.nodeDest + "fontawesome-free/sprites"));

    gulp.src(paths.nodeRoot + "@fortawesome/fontawesome-free/svgs/*")
        .pipe(gulp.dest(paths.nodeDest + "fontawesome-free/svgs"));

    return gulp.src(paths.nodeRoot + "@fortawesome/fontawesome-free/webfonts/*")
        .pipe(gulp.dest(paths.nodeDest + "fontawesome-free/webfonts"));
});
// Copy jQuery Unobtrusive Validation distribution files to wwwroot
gulp.task("lib:bootswatch", function () {
    return gulp.src(paths.nodeRoot + "bootswatch/dist/*/*")
        .pipe(gulp.dest(paths.nodeDest + "bootswatch/dist"));
});

// Client-side library tasks
gulp.task("lib", gulp.series(
    [
        "lib:bootstrap",
        "lib:jquery",
        "lib:jquery-validation",
        "lib:jquery-validation-unobtrusive",
        "lib:fontawesome-free",
        "lib:bootswatch"
    ]
));

// A 'default' task is required by Gulp v4
gulp.task("default", gulp.series(["min", "lib"]));
