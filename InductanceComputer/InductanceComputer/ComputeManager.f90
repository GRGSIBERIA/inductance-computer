    module ComputeManager
    USE VectorClass
    implicit none
    
    type, public :: point
        type(vector) position   ! 点の座標
        real flux_density       ! 点にかかる磁束密度
    end type
    
    type, public :: coil
        type(vector) position   ! コイルの位置
        type(vector) front      ! コイルの正面
        type(vector) right      ! コイルの右手
    end type
    
    type, public :: coil_info
        real radius ! コイルの半径
        real sigma  ! コイルの電荷密度
        real gamma
    end type
    
    real, allocatable, dimension(:) :: times
    type(position), allocatable, dimension(:,:) :: points
    type(coil), allocatable, dimension(:,:)     :: coils
    type(coil_info), allocatable, dimension(:)  :: coil_infos
    
    contains
    function GetArray(n)
        integer, intent(in) :: n
        real GetArray(n)
        GetArray = 0
    end function GetArray
    
    subroutine LoadPointCSV(path)
        character(len=256), intent(in) :: path
        integer, parameter :: FD = 200
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
    
    end module ComputeManager