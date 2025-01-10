mergeInto(LibraryManager.library, {
    gameOver: function (result, score) {
        const result = JSON.stringify({
            result,
            score
        });

        console.log(`Game Over: ${result}`);
    }
});