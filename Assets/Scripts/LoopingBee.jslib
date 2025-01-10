mergeInto(LibraryManager.library, {
    gameOver: function (result, score) {
        console.log(`Game Over: {"result": "${result}", "score": "${score}"}`);
    }
});