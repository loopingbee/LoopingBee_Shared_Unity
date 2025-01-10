mergeInto(LibraryManager.library, {
    gameOver: function (result, score) {
        const message = JSON.stringify({
            result: result == 1,
            score
        });

        console.log(`Game Over: ${message}`);
    }
});