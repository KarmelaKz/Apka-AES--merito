#!/bin/bash

DATE=$(date +"%Y-%m-%d_%H-%M")
OUTPUT="/home/kara/scan_$DATE.txt"

echo "[INFO] Skanowanie Nmap o $DATE" | tee $OUTPUT
/usr/bin/nmap -sV 127.0.0.1 | tee -a $OUTPUT
echo "[INFO] Skan zakończony. Wynik zapisano w pliku $OUTPUT" | tee -a $OUTPUT
