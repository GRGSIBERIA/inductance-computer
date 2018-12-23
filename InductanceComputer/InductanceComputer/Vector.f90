    module VectorClass
    implicit none
    
    ! vector型の宣言
    type, public :: vector
        real :: xyzw(4) = (/ 0.0, 0.0, 0.0, 0.0 /)
    contains
        procedure :: dot => vector_dot
        procedure :: cross => vector_cross
        procedure :: length => vector_length
        procedure :: normalize => vector_normalize
        procedure :: conj => vector_conjugate
        procedure :: reciprocal => vector_reciprocal
        procedure :: rotate => vector_rotate
        procedure :: show => vector_show
    end type vector
    
    ! コンストラクタの宣言
    
    ! 演算子オーバーロード
    public operator (+)
    interface operator (+)
        module procedure vector_add
    end interface
    
    public operator (-)
    interface operator (-)
        module procedure vector_sub
    end interface
    
    public operator (*)
    interface operator (*)
        module procedure vector_mul_vr_real, vector_mul_rv_real, vector_mul_vr_int, vector_mul_rv_int
    end interface
    
    ! 関数の実装と宣言
    contains
    
    type(vector) function vector_add(lhs, rhs)
        implicit none
        type(vector), intent(in) :: lhs, rhs
        integer i
        DO i = 1, 4
            vector_add%xyzw(i) = lhs%xyzw(i) + rhs%xyzw(i)
        END DO
    end function vector_add
    
    type(vector) function vector_sub(lhs, rhs)
        implicit none
        type(vector), intent(in) :: lhs, rhs
        integer i
        DO i = 1, 4
            vector_sub%xyzw(i) = lhs%xyzw(i) - rhs%xyzw(i)
        END DO
    end function vector_sub
    
    type(vector) function vector_mul_vr_real(lhs, rhs)
        implicit none
        type(vector), intent(in) :: lhs
        real, intent(in) :: rhs
        integer i
        DO i = 1, 4
            vector_mul_vr_real%xyzw(i) = lhs%xyzw(i) * rhs
        END DO
    end function vector_mul_vr_real
    
    type(vector) function vector_mul_rv_real(lhs, rhs)
        implicit none
        type(vector), intent(in) :: rhs
        real, intent(in) :: lhs
        integer i
        DO i = 1, 3
            vector_mul_rv_real%xyzw(i) = lhs * rhs%xyzw(i)
        END DO
    end function vector_mul_rv_real
    
    type(vector) function vector_mul_vr_int(lhs, rhs)
        implicit none
        type(vector), intent(in) :: lhs
        integer, intent(in) :: rhs
        integer i
        DO i = 1, 4
            vector_mul_vr_int%xyzw(i) = lhs%xyzw(i) * real(rhs)
        END DO
    end function vector_mul_vr_int
    
    type(vector) function vector_mul_rv_int(lhs, rhs)
        implicit none
        type(vector), intent(in) :: rhs
        integer, intent(in) :: lhs
        integer i
        DO i = 1, 3
            vector_mul_rv_int%xyzw(i) = real(lhs) * rhs%xyzw(i)
        END DO
    end function vector_mul_rv_int

    real function vector_dot(lhs, rhs)
        implicit none
        class(vector), intent(in) :: lhs
        class(vector), intent(in) :: rhs
        integer i
        vector_dot = 0.0
        DO i = 1, 3
            vector_dot = vector_dot + lhs%xyzw(i) * rhs%xyzw(i)
        END DO
    end function vector_dot
    
    type(vector) function vector_cross(lhs, rhs)
        implicit none
        class(vector), intent(in) :: rhs, lhs
        integer i
        vector_cross%xyzw(1) = rhs%xyzw(2) * lhs%xyzw(3) - lhs%xyzw(2) * rhs%xyzw(3)
        vector_cross%xyzw(2) = rhs%xyzw(3) * lhs%xyzw(1) - lhs%xyzw(3) * rhs%xyzw(1)
        vector_cross%xyzw(3) = rhs%xyzw(1) * lhs%xyzw(2) - lhs%xyzw(1) * rhs%xyzw(2)
        
        vector_cross%xyzw(4) = 0.0  ! 実部
        DO i = 1, 3
            vector_cross%xyzw(4) = vector_cross%xyzw(4) + rhs%xyzw(i) * lhs%xyzw(i)
        END DO

        ! 実部がマイナスになるかどうか定かではない
    end function vector_cross
    
    real function vector_length(this)
        implicit none
        class(vector), intent(in) :: this
        integer i
        vector_length = 0.0
        DO i = 1, 4
            vector_length = vector_length + this%xyzw(i) * this%xyzw(i)
        END DO
        vector_length = sqrt(vector_length)
    end function vector_length
    
    type(vector) function vector_normalize(this)
        implicit none
        class(vector), intent(in) :: this
        vector_normalize = 1.0 / this.length() * this
    end function vector_normalize
    
    ! q の共役 q*
    type(vector) function vector_conjugate(this)
        implicit none
        class(vector), intent(in) :: this
        integer i
        DO i = 1, 3
            vector_conjugate%xyzw(i) = -this%xyzw(i)
        END DO
        vector_conjugate%xyzw(4) = this%xyzw(4)
    end function vector_conjugate
    
    ! q の逆数 q^-1
    type(vector) function vector_reciprocal(this)
        implicit none
        class(vector), intent(in) :: this
        vector_reciprocal = vector_normalize(this.conj())
    end function vector_reciprocal
    
    ! 回転の連結
    type(vector) function vector_join(lhs, rhs)
        implicit none
        class(vector), intent(in) :: lhs, rhs
        vector_join = vector_cross(lhs.normalize(), rhs.normalize())
    end function vector_join
    
    ! 回転, p は実部がゼロなので回転してもベクトルに戻る
    type(vector) function vector_rotate(lhs, rhs)
        implicit none
        class(vector), intent(in) :: lhs, rhs
        type(vector) temp
        temp = lhs.normalize()
        vector_rotate = temp.reciprocal()           ! q^-1
        vector_rotate = vector_rotate.cross(rhs)    ! q^-1 p
        vector_rotate = vector_rotate.cross(temp)   ! q^-1 p q
    end function vector_rotate
    
    ! ベクトルの中身を表示
    subroutine vector_show(this)
        implicit none
        class(vector), intent(in) :: this
        print *, this%xyzw(1), this%xyzw(2), this%xyzw(3), this%xyzw(4)
    end subroutine vector_show
    
    end module VectorClass