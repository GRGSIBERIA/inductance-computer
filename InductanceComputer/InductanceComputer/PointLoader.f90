    module PointLoader
    USE VectorClass
    implicit none
    
    type, public :: point
        type(vector) position   ! 点の座標
        real flux_density       ! 点にかかる磁束密度
    end type
    
    real, allocatable, dimension(:) :: times
    type(vector), allocatable, dimension(:,:) :: points
    
    contains
    function GetArray(n)
        integer, intent(in) :: n
        real GetArray(n)
        GetArray = 0
    end function GetArray
    
    subroutine LoadPointCSV(path)
        character(len=256), intent(in) :: path
        integer, parameter :: FD = 17
        integer time_count, point_count, time_id, point_id
        open (FD, file=path, status="old")
        
        ! ヘッダの読み取りと領域の確保
        read (FD, *) time_count, point_count
        allocate(times(time_count))
        allocate(points(time_count, point_count))
        
        DO time_id = 1, time_count
            DO point_id = 1, point_count
                
            END DO
        END DO
        
        close (FD)
    end subroutine LoadPointCSV
    
    subroutine DisposePointLoader()
        deallocate(times)
        deallocate(points)
    end subroutine DisposePointLoader
    
    end module PointLoader