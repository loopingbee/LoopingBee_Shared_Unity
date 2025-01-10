mergeInto(LibraryManager.library, {
    gameOver: function (result, score) {
        const message = JSON.stringify({
            result,
            score
        });

        console.log(`Game Over: ${message}`);
    }
});