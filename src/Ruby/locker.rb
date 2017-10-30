#/usr/bin/env ruby
# encoding: UTF-8

require 'io/console'

BEGIN {
FOLDER   = "\u{00a0}"
PASSWORD = "528819"

$> << %{\e[35m
              ／￣￣￣￣￣￣￣￣￣￣￣￣￣￣
  　　　　　　|　PLEASE INSERT THE PASSWORD
              ＼＿＿  ＿＿＿＿＿＿  
  ._               ∨             _.-|
  |_~~`--._                 _.-~   /
    ~-._   ~-._.-~~~~~~~-.-~    _.~
        ~-._ /             \\\_.-~ 
            |  .-.   .-.   |      
            |. |_| . |_|   |         __.-|
            /    .__,      |    _.--~    |
            \\             /_--~~  \\     / 
            /~-._______.-~  \\     |____|      
           |  /         \\    |        /
            ~|_       |_|_-~~        / 
             | ~-\\\_/-~    |        _~ 
             |   |   |    /     _-~ 
              |  |  /    |__---~ 
              |,_|,_|____(   

\u{1f512}: \e[0m} }

trace_var :$locked, proc { |val| val = 0 }

class String
  define_method(:hide)   { `attrib +h +s #{self}` }
  define_method(:show)   { `attrib -h -s #{self}` }

  def red;              "\e[31m#{self}\e[0m" end
  def blue;             "\e[34m#{self}\e[0m" end
  def green;            "\e[32m#{self}\e[0m" end
  def purple;           "\e[35m#{self}\e[0m" end
end

define_method(:lock)   { `mkdir locked && attrib +h +s locked` }
define_method(:unlock) { `rmdir locked` }

define_method(:has_autorun?) { open('autorun.inf', 'wb') { |f| f << %{[autorun]\nicon=i.ico} }; 'autorun.inf'.hide }

has_autorun? if !test ?e, 'autorun.inf'

define_method(:main) { |input| test ?e, FOLDER ? (input[/#{PASSWORD}/im] ? locker : \
  (abort "wrong password \\_(ツ)_/¯".red)) : (Dir.mkdir(FOLDER); main(input)) }

def locked
  print %{
 _            _            _ 
| |          | |          | |
| | ___   ___| | _____  __| |
| |/ _ \\ / __| |/ / _ \\/ _` |
| | (_) | (__|   <  __/ (_| |
|_|\\___/ \\___|_|\\_\\___|\\__,_|
  }.green
end
def unlocked
  print %{
             _            _            _ 
            | |          | |          | |
 _   _ _ __ | | ___   ___| | _____  __| |
| | | | '_ \\| |/ _ \\ / __| |/ / _ \\/ _` |
| |_| | | | | | (_) | (__|   <  __/ (_| |
 \\__,_|_| |_|_|\\___/ \\___|_|\\_\\___|\\__,_|
  }.blue
end

def locker
  system 'cls'
  Dir.exists?('locked') ? (FOLDER.show; unlock; $locked = false; `explorer #{FOLDER}`) : (FOLDER.hide; lock; $locked = true)
  abort $locked ? locked.to_s : unlocked.to_s
end

trap("SIGINT") { throw :ctrl_c }
catch :ctrl_c do
  main(STDIN.noecho(&:gets).chomp)
end

at_exit { abort $! ? 'uh, something broke' : 'Bye!' }