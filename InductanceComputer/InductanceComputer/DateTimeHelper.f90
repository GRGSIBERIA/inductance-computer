    module DateTimeHelper
    implicit none
    
    contains
    
    ! 現在の時刻を返す関数
    function GetDateTime()
        implicit none
        character date*8, time*10, zone*5
        integer dateTime(8)
        character GetDateTime*23
        
        CALL DATE_AND_TIME(date, time, zone, dateTime)
        GetDateTime = date(1:4) // "/" // date(5:6) // "/" // date(7:8) // "-" // time(1:2) // ":" // time(3:4) // ":" // time(5:10)
        
    end function GetDateTime
    
    function GetDateTimeB()
        implicit none
        character GetDateTimeB*29
        GetDateTimeB = "[" // GetDateTime() // "] -- "
    end function GetDateTimeB
    
    end module DateTimeHelper