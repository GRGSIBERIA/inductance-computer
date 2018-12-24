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
    type(point), allocatable, dimension(:,:) :: points
    type(coil), allocatable, dimension(:,:)     :: coils
    type(coil_info), allocatable, dimension(:)  :: coil_infos
    
    contains
    
    subroutine DisposePointLoader()
        deallocate(times)
        deallocate(points)
        deallocate(coils)
        deallocate(coil_infos)
    end subroutine DisposePointLoader
    
    end module ComputeManager