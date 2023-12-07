import 'package:flutter/material.dart';

class CapsuleTitle extends StatelessWidget {
  const CapsuleTitle({super.key, required this.name});

  final String name;

  @override
  Widget build(BuildContext context) {
    return Text(
      name,
      style: const TextStyle(
        fontSize: 12,
        fontWeight: FontWeight.bold,
      ),
    );
  }
}
