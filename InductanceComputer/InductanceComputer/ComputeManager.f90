    module ComputeManager
    USE VectorClass
    USE DateTimeHelper
    implicit none
    
    type, public :: point
        type(vector) position   ! 点の座標
        real flux_density       ! 点にかかる磁束密度
    end type point
    
    type, public :: coil
        type(vector) position   ! コイルの位置
        type(vector) front      ! コイルの正面
        type(vector) right      ! コイルの右手
    end type coil
    
    type, public :: coil_info
        real radius ! コイルの半径
        real sigma  ! コイルの電荷密度
        real gamma
    end type coil_info
    
    real, allocatable, dimension(:) :: times
    type(point), allocatable, dimension(:,:) :: points
    type(coil), allocatable, dimension(:,:)     :: coils
    type(coil_info), allocatable, dimension(:)  :: coil_infos
    
    contains
    
    subroutine DisposeComputeManager()
        implicit none
        deallocate(times)
        deallocate(points)
        deallocate(coils)
        deallocate(coil_infos)
        print *, GetDateTimeB(), "Done disposed compute manager"
    end subroutine DisposeComputeManager
    
    end module ComputeManager