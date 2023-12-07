import 'package:flutter/material.dart';

class CapsulePrice extends StatelessWidget {
  const CapsulePrice({
    super.key,
    required this.price,
  });

  final double price;

  @override
  Widget build(BuildContext context) {
    return Text(
      '€ ${price.toStringAsFixed(2)}',
      style: const TextStyle(
        fontSize: 12,
        color: Color(0xFF3d8705),
        fontWeight: FontWeight.bold,
      ),
    );
  }
}
