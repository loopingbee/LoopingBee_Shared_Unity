mergeInto(LibraryManager.library, {
    gameOver: function (result, score, showcaseDelay) {
        const message = JSON.stringify({
            result: result == 1,
            score,
            showcaseDelay
        });

        console.log(`Game Over: ${message}`);
    }
});