mergeInto(LibraryManager.library, {
    gameOver: function (result, score, allowContinue, showcaseDelay) {
        const message = JSON.stringify({
            result: result == 1,
            score,
            showcaseDelay,
            allowContinue: allowContinue == 1
        });

        // For backwards compatibility with App 0.8.X
        console.log(`Game Over: ${message}`);
        console.log(`[S&P SDK] Game Over: ${message}`);
    },
    purchaseProduct: function (product_id, uuid)
    {
        const message = JSON.stringify({
            action: "purchase",
            data: {
                product_id: UTF8ToString(product_id),
                uuid: UTF8ToString(uuid)
            }
        })

        // For backwards compatibility with App 0.8.X
        console.log(`Action: ${message}`);
        console.log(`[S&P SDK] Action: ${message}`);
    },
    gameStarted: function () {
        console.log(`[S&P SDK] Game Started`);
    }
});