!  InductanceComputer.f90 
!
!  関数:
!  InductanceComputer - コンソール・アプリケーションのエントリーポイント。
!

!****************************************************************************
!
!  プログラム: InductanceComputer
!
!  目的:  コンソール・アプリケーションのエントリーポイント。
!
!****************************************************************************

    program InductanceComputer
    USE VectorClass
    implicit none
    type(vector) a, b, c
    a = vector(3)
    b = vector(1)
    c = vector(5)
    ! 変数
    c%xyzw = (/ 1.0, 2.0, 3.0, 4.0 /)
    c = c.conj()

    ! InductanceComputer の本文
    print *, 'Hello World'
    CALL a.show()
    CALL b.show()
    CALL c.show()

    end program InductanceComputer

