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
    USE CSVManager
    implicit none
    
    print *, "hello"
    CALL LoadPointCSV("TEST_POINT.CSV")
    CALL LoadCoilCSV("TEST_COIL.CSV")
    
    end program InductanceComputer

