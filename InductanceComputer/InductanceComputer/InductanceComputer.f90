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
    CALL LoadPointCSV("TEST.CSV")
    
    end program InductanceComputer

