fn read<T: std::str::FromStr>() -> T {
    let mut line = String::new();
    std::io::stdin().read_line(&mut line).ok();
    line.trim().parse().ok().unwrap()
}

struct Data {
    num_times: f32,
    num_time_divisions: i32,
    coil_height: f32,
    top_of_coil: f32,
    start_move: f32,
    end_move: f32,
}

fn main() {
    println!("Number of record times:");
    let num_times: f32 = read();

    println!("Number of time divisions:");
    let num_time_divisions: i32 = read();

    println!("Coil's height:");
    let height: f32 = read();

    println!("Wire's height position of top of the coil:");
    let top_of_coil: f32 = read();

    println!("Start of horizontal movement of wire:");
    let start_move: f32 = read();

    println!("End of horizontal movement of wire:");
    let end_move: f32 = read();

    let data = Data {
        num_times: num_times,
        num_time_divisions: num_time_divisions,
        coil_height: height,
        top_of_coil: top_of_coil,
        start_move: start_move,
        end_move: end_move
        };
    
    println!("Number of times: {} (sec)", data.num_times);
    println!("Number of time divisions: {}", data.num_time_divisions);
    println!("Coil's height: {} (mm)", data.coil_height);
    println!("Wire's height position of top of the coil: {} (mm)", data.top_of_coil);
    println!("Start of horizontal movement of wire: {} (mm)", data.start_move);
    println!("End of horizontal movement of wire: {} (mm)", data.end_move);
}
