    module VectorClass
    implicit none
    
    ! vector型の宣言
    type, public :: vector
        real xyzw(4)
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
    interface vector
        module procedure init_vector_zero, init_vector, init_vector_scalar, init_vector_three, init_vector_four
    end interface vector
    
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
        module procedure vector_mul_vr, vector_mul_rv
    end interface
    
    ! 関数の実装と宣言
    contains
    type(vector) function init_vector_zero() result(this)
        implicit none
        integer i
        DO i = 1, 4
            this%xyzw(i) = 0.0
        END DO
    end function init_vector_zero
    
    type(vector) function init_vector(rhs) result(this)
        implicit none
        type(vector), intent(in) :: rhs
        integer i
        DO i = 1, 4
            this%xyzw(i) = rhs%xyzw(i)
        END DO
    end function init_vector
    
    type(vector) function init_vector_scalar(x) result(this)
        implicit none
        real, intent(in) :: x
        this = vector(x, x, x)
    end function init_vector_scalar
    
    type(vector) function init_vector_three(x, y, z) result(this)
        implicit none
        real, intent(in) :: x, y, z
        this%xyzw(1) = x
        this%xyzw(2) = y
        this%xyzw(3) = z
        this%xyzw(4) = 0
    end function init_vector_three
    
    type(vector) function init_vector_four(x, y, z, w) result(this)
        implicit none
        real, intent(in) :: x, y, z, w
        this%xyzw(1) = x
        this%xyzw(2) = y
        this%xyzw(3) = z
        this%xyzw(4) = w
    end function init_vector_four
    
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
    
    type(vector) function vector_mul_vr(lhs, rhs)
        implicit none
        type(vector), intent(in) :: lhs
        real, intent(in) :: rhs
        integer i
        DO i = 1, 4
            vector_mul_vr%xyzw(i) = lhs%xyzw(i) * rhs
        END DO
    end function vector_mul_vr
    
    type(vector) function vector_mul_rv(lhs, rhs)
        implicit none
        type(vector), intent(in) :: rhs
        real, intent(in) :: lhs
        integer i
        DO i = 1, 3
            vector_mul_rv%xyzw(i) = lhs * rhs%xyzw(i)
        END DO
    end function vector_mul_rv

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
    
    subroutine vector_show(this)
        implicit none
        class(vector), intent(in) :: this
        print *, this%xyzw(1), this%xyzw(2), this%xyzw(3), this%xyzw(4)
    end subroutine vector_show
    
    end module VectorClass