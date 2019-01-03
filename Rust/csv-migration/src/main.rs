fn read_i32() -> i32 {
    let mut line = String::new();
    let scan = std::io::stdin();
    let _ = scan.read_line(&mut line);
    let vec: Vec<&str> = line.split_whitespace().collect();
    let num: i32 = vec[0].parse().unwrap();
    num
}

struct Data {
    num_times: i32,
    num_time_divisions: i32,
    coil_height: i32,
    top_of_the_coil: i32,
    range_start: f32,
    range_end: f32,
}

fn main() {
    println!("Number of times:");

    println!("Number of time division:");

    println!("Coil's height:");

    println!("Wire's position of top of the coil:");

    println!("Start of horizontal movement of wire:");

    println!("End of horizontal movement of wire:");
}
