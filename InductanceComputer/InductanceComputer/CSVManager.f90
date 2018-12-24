﻿
    INCLUDE "DateTimeHelper.f90"
    
    module CSVManager
    USE ComputeManager
    USE DateTimeHelper
    implicit none
    
    private CreateArray
    public LoadPointCSV
    
    contains
    ! 任意の長さの配列を返す関数
    function CreateArray(n)
        integer, intent(in) :: n
        real CreateArray(n)
        CreateArray = 0
    end function CreateArray
    
    ! 点群のCSVファイルを読み込む
    subroutine LoadPointCSV(path)
        implicit none
        character(*), intent(in) :: path
        integer, parameter :: FD = 200
        integer time_count, point_count, time_id, point_id
        real, allocatable :: line_data(:)
        
        open (FD, file=path, status="old")
        
        ! ヘッダの読み取りと領域の確保
        read (FD, *) time_count, point_count
        allocate(times(time_count))                 ! ComputeManager.times の初期化
        allocate(points(time_count, point_count))   ! ComputeManager.points の初期化
        allocate(line_data(point_count * 3 + 1))    ! 一時変数の領域を確保
        
        print *, GetDateTimeB(), "Import Point CSV: ", path
        
        ! データを入力
        DO time_id = 1, time_count
            read (FD, *), line_data
            times(time_id) = line_data(1)
            DO point_id = 1, point_count
                points(time_id, point_id) = point(vector(line_data((point_id-1) * 3 + 2:(point_id-1) * 3 + 4)), 0)
            END DO
        END DO
        
        print *, GetDateTimeB(), "Done Import Point CSV"
        
        deallocate(line_data)
        close (FD)
    end subroutine LoadPointCSV
    
    ! コイルのCSVファイルを読み込む
    subroutine LoadCoilCSV(path)
        implicit none
        character(*), intent(in) :: path
        integer, parameter :: FD = 200
        integer time_count, coil_count
        real, allocatable :: line_data(:)
        real info_data(3)
        integer time_id, coil_id, temp
        
        open (FD, file=path, status="old")
        
        ! ヘッダ行の読み取り
        read (FD, *) time_count, coil_count
        allocate(coils(time_count, coil_count))
        allocate(coil_infos(coil_count))
        
        ! 点群とコイルの時系列が一致しない
        if (size(times) .ne. time_count) GOTO 100
        
        print *, GetDateTimeB(), "Import Coil CSV: ", path
        
        ! コイルの設定データの読み取り
        DO coil_id = 1, coil_count
            read (FD, *), info_data
            coil_infos(coil_id)%radius = info_data(1)
            coil_infos(coil_id)%sigma = info_data(2)
            coil_infos(coil_id)%gamma = info_data(3)
        END DO
        
        allocate(line_data(coil_count*3*3+1)) ! コイル数 * 座標，前，右ベクトル + 時間
        
        ! データの入力
        DO time_id = 1, time_count
            read (FD, *), line_data
            IF (times(time_id) .ne. line_data(1)) GOTO 100  ! 点群とコイルの時系列データがおかしかったらエラーを出して終了する
            
            DO coil_id = 1, coil_count
                temp = coil_id - 1  ! 何度も出てくるので一時変数に退避させる
                coils(time_id, coil_id) = coil(vector(line_data(temp*9+2:temp*9+4)), vector(line_data(temp*9+5:temp*9+7)), vector(line_data(temp*9+8:temp*9+10)))
            END DO
        END DO
        GOTO 200
        
        ! エラーハンドリング
100     continue
        print *, "Do not match times from Coil and Point"
        stop
        
200     continue
        
        print *, GetDateTimeB(), "Done Import Coil CSV"
        
        deallocate(line_data)
        close (FD)
    end subroutine LoadCoilCSV
    
    end module CSVManager