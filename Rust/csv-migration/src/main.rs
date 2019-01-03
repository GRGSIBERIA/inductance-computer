use std::str::FromStr;
use std::fmt::Debug;

fn read_line<T>() -> Vec<T>
where T: FromStr, <T as FromStr>::Err : Debug {
    let mut s = String::new();
    std::io::stdin().read_line(&mut s).unwrap();
    s.trim().split_whitespace().map(|c| T::from_str(c).unwrap()).collect()
}

fn main() {
    
    let mut s: String = String::new();
    std::io::stdin().read_line(&mut s).unwrap();
    let num: i32 = s.parse().unwrap();

    println!("{}", num);

    println!("Number of times:");

    println!("Number of time division:");

    println!("Coil's height:");

    println!("Wire's position of top of the coil:");

    println!("Range of horizontal movement of wire:");
}
