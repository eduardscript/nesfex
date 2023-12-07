import 'package:flutter/material.dart';

class CapsuleImage extends StatelessWidget {
  const CapsuleImage({
    super.key,
    required this.imageUrl,
  });

  final String imageUrl;

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 5, horizontal: 10),
      child: Image.network(
        imageUrl,
        width: 50,
      ),
    );
  }
}
