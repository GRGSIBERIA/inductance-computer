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

    INCLUDE "Vector.f90"
    INCLUDE "ComputeManager.f90"
    INCLUDE "CSVManager.f90"
    
    program InductanceComputer
    USE VectorClass
    USE CSVManager
    implicit none
    
    print *, "hello"
    CALL LoadPointCSV("TEST_POINT.CSV")
    CALL LoadCoilCSV("TEST_COIL.CSV")
    
    CALL DisposeComputeManager()
    
    end program InductanceComputer

