// Thai

jQuery.extend( jQuery.fn.pickadate.defaults, {
    monthsFull: [ 'มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม' ],
    monthsShort: [ 'ม.ค.', 'ก.พ.', 'มี.ค.', 'เม.ย.', 'พ.ค.', 'มิ.ย.', 'ก.ค.', 'ส.ค.', 'ก.ย.', 'ต.ค.', 'พ.ย.', 'ธ.ค.' ],
    weekdaysFull: [ 'อาทติย', 'จันทร', 'องัคาร', 'พุธ', 'พฤหสั บดี', 'ศกุร', 'เสาร' ],
    weekdaysShort: [ 'อ.', 'จ.', 'อ.', 'พ.', 'พฤ.', 'ศ.', 'ส.' ],
    today: 'วันนี้',
    clear: 'ลบ',
    format: 'd mmmm yyyy',
    formatSubmit: 'yyyy/mm/dd'
});

try {
    jQuery.extend(jQuery.fn.pickatime.defaults, {
        clear: 'ลบ'
    });
} catch (e) {

}
