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
    type(vector) v
    v = vector(3, 5, 3, 2)
    ! 変数

    ! InductanceComputer の本文
    print *, 'Hello World'
    CALL v.show()

    end program InductanceComputer

