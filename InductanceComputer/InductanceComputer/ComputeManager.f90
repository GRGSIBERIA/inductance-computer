    module ComputeManager
    USE VectorClass
    implicit none
    
    type, public :: point
        type(vector) position   ! 点の座標
        real flux_density       ! 点にかかる磁束密度
    end type point
    
    interface point
        module procedure init_point
    end interface point
    
    type, public :: coil
        type(vector) position   ! コイルの位置
        type(vector) front      ! コイルの正面
        type(vector) right      ! コイルの右手
    end type coil
    
    interface coil
        module procedure init_coil
    end interface coil
    
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
    
    subroutine DisposePointLoader()
        implicit none
        deallocate(times)
        deallocate(points)
        deallocate(coils)
        deallocate(coil_infos)
    end subroutine DisposePointLoader
    
    type(point) function init_point() result(this)
        implicit none
        this%position = vector()
    end function init_point
    
    type(coil) function init_coil() result(this)
        implicit none
        this%position = vector()
        this%front = vector()
        this%right = vector()
    end function init_coil
    
    end module ComputeManager