    module CSVManager
    USE ComputeManager
    implicit none
    
    contains
    function CreateArray(n)
        integer, intent(in) :: n
        real CreateArray(n)
        CreateArray = 0
    end function CreateArray
    
    subroutine LoadPointCSV(path)
        character(len=256), intent(in) :: path
        integer, parameter :: FD = 200
        integer time_count, point_count, time_id, point_id
        real, allocatable :: line_data(:)
        
        open (FD, file=path, status="old")
        
        ! ヘッダの読み取りと領域の確保
        read (FD, *) time_count, point_count
        allocate(times(time_count))                 ! ComputeManager.times の初期化
        allocate(points(time_count, point_count))   ! ComputeManager.points の初期化
        allocate(line_data(point_count * 3 + 1))    ! 一時変数の領域を確保
        
        ! データを入力
        DO time_id = 1, time_count
            read (FD, *), line_data
            times(time_id) = line_data(1)
            DO point_id = 1, point_count
                points(time_id, point_id)%position%xyzw(1:3) = line_data(point_id * 3 + 1:point_id * 3 + 3)
            END DO
        END DO
        
        deallocate(line_data)
        close (FD)
    end subroutine LoadPointCSV
    
    end module CSVManager