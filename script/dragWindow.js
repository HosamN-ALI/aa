$.fn.dragWindow = function () {
    
    var mY = 0;
    var startDrag = false;
    var isFirst = true;
    var scrollMovement = 10;
    var isScrollUp = true;    
    var mX = 0;

    $(this).on('mousedown', function (e) {
        
        if ($(e.target).is('input') || $(e.target).is('textarea'))
        {
            $(e.target).focus();
        }
        startDrag = true;
        e.originalEvent.preventDefault();
        var oldStyle = $('body').attr('style');
        //console.log(oldStyle);
        oldStyle = oldStyle.replace('cursor: -webkit-grab; cursor: -moz-grab;', '');
        $('body').attr('style', oldStyle + 'cursor:-webkit-grabbing; cursor: -moz-grabbing;');
        mY = e.pageY;
        mX = e.pageX;
    })
    .on('mouseup', function (e) {
        startDrag = false;
        e.originalEvent.preventDefault();
        var oldStyle = $('body').attr('style');
        //console.log(oldStyle);
        oldStyle = oldStyle.replace('cursor:-webkit-grabbing; cursor: -moz-grabbing;', '');
        $('body').attr('style', oldStyle + 'cursor: -webkit-grab; cursor: -moz-grab; ');
        isFirst = true;
    })
    .on('mousemove', function (e) {        
        
        if (startDrag) {
            {//Up_Down
                var scrollTopPx = $(this).scrollTop();
                console.log('X=' + scrollTopPx);
                // moving upward
                if (mY < e.pageY) {
                    isScrollUp = false;
                    // moving downward
                } else {
                    isScrollUp = true;
                }

                if (isScrollUp) {
                    $(this).scrollTop(scrollTopPx + scrollMovement);
                    // set new mY after doing test above       
                    mY = e.pageY + scrollMovement;
                }
                else {
                    $(this).scrollTop(scrollTopPx - scrollMovement);
                    // set new mY after doing test above       
                    mY = e.pageY - scrollMovement;
                }
            }

            {//Left_right
                var scrollLeftPx = $(this).scrollLeft();
                console.log('Y=' + scrollLeftPx);

                // moving upward
                if (mX < e.pageX) {
                    isScrollUp = false;
                    // moving downward
                } else {
                    isScrollUp = true;
                }

                if (isScrollUp) {
                    $(this).scrollLeft(scrollLeftPx + scrollMovement);
                    // set new mX after doing test above       
                    mX = e.pageX + scrollMovement;
                }
                else {
                    $(this).scrollLeft(scrollLeftPx - scrollMovement);
                    // set new mX after doing test above       
                    mX = e.pageX - scrollMovement;
                }
            }
        }        
    });


    function sleep(milliseconds) {
        var start = new Date().getTime();
        for (var i = 0; i < 1e7; i++) {
            if ((new Date().getTime() - start) > milliseconds) {
                break;
            }
        }
    }
};