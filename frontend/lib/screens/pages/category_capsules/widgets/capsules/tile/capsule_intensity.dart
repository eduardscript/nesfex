import 'package:flutter/material.dart';

class CapsuleIntensity extends StatelessWidget {
  const CapsuleIntensity({super.key, required this.intensity});

  final int intensity;

  @override
  Widget build(BuildContext context) {
    return Text.rich(
      TextSpan(
        text: 'Intensity ',
        style: const TextStyle(
          color: Color(0xFF876c43),
          fontSize: 11,
        ),
        children: [
          TextSpan(
            text: '$intensity',
            style: const TextStyle(
              fontWeight: FontWeight.bold,
            ),
          ),
        ],
      ),
    );
  }
}
