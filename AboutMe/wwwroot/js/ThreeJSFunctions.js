var container, clock;
var camera, scene, renderer, mixer;
var particles; // Declare particles as a global variable

function render() {
    var delta = clock.getDelta();
    if (mixer !== undefined) {
        mixer.update(delta);
    }
    renderer.render(scene, camera);
}

function rotateCamera() {
    // Adjust the camera's position to make it slowly rotate
    camera.position.x = Math.cos(Date.now() * 0.00004) * 20;
    camera.position.z = Math.sin(Date.now() * 0.00004) * 20;
    camera.lookAt(scene.position);
}

function getRandomBlueColor() {
    // Generate a random shade of blue
    const hue = 0.6 + Math.random() * 0.2; // Adjust the hue for blue shades
    const saturation = 1;
    const lightness = 0.5 + Math.random() * 0.25; // Adjust the lightness for variation
    return new THREE.Color().setHSL(hue, saturation, lightness);
}

function moveParticles() {
    const positions = particles.geometry.attributes.position.array;
    const spread = 800;
    const movementFactor = 0.0007; // Adjust this value to control the particle movement speed

    for (let i = 0; i < positions.length; i += 3) {
        // Update particle positions with slower random movement but larger range
        positions[i] += (Math.random() - 0.5) * 10 * movementFactor; // Increase the range (10) here
        positions[i + 1] += (Math.random() - 0.5) * 10 * movementFactor; // Increase the range (10) here
        positions[i + 2] += (Math.random() - 0.5) * 10 * movementFactor; // Increase the range (10) here

        // Wrap particles around if they go too far
        if (Math.abs(positions[i]) > spread || Math.abs(positions[i + 1]) > spread || Math.abs(positions[i + 2]) > spread) {
            positions[i] = (Math.random() - 0.5) * spread;
            positions[i + 1] = (Math.random() - 0.5) * spread;
            positions[i + 2] = (Math.random() - 0.5) * spread;
        }
    }

    // Update the buffer geometry
    particles.geometry.attributes.position.needsUpdate = true;
}

function loadScene() {
    container = document.getElementById('threejscontainer');
    if (!container) {
        return;
    }

    scene = new THREE.Scene();

    camera = new THREE.PerspectiveCamera(25, container.clientWidth / container.clientHeight, 1, 1000);
    camera.position.set(0, 5, 20); // Adjusted camera position
    camera.lookAt(scene.position);

    clock = new THREE.Clock();

    const geometry = new THREE.BufferGeometry();

    const vertices = [];
    const colors = []; // Store colors for each particle

    const particleCount = 90000;
    const spread = 90; // Increase the spread value

    for (let i = 0; i < particleCount; i++) {
        const x = (Math.random() - 0.5) * spread;
        const y = (Math.random() - 0.5) * spread;
        const z = (Math.random() - 0.5) * spread;
        vertices.push(x, y, z);

        // Generate a random shade of blue for each particle
        const color = getRandomBlueColor();
        colors.push(color.r, color.g, color.b);
    }

    geometry.setAttribute('position', new THREE.Float32BufferAttribute(vertices, 3));
    geometry.setAttribute('color', new THREE.Float32BufferAttribute(colors, 3));

    const material = new THREE.PointsMaterial({ vertexColors: THREE.VertexColors, size: 3, opacity: 0.4, transparent: true }); // Adjust opacity and transparent properties

    particles = new THREE.Points(geometry, material); // Assign particles to the global variable
    scene.add(particles);

    var ambientLight = new THREE.AmbientLight(0xffffff, 0.2);
    scene.add(ambientLight);

    var pointLight = new THREE.PointLight(0xffffff, 0.8);
    scene.add(camera);
    camera.add(pointLight);

    renderer = new THREE.WebGLRenderer({ antialias: true });
    renderer.setPixelRatio(window.devicePixelRatio);
    renderer.setSize(container.clientWidth, container.clientHeight);

    while (container.lastElementChild) {
        container.removeChild(container.lastElementChild);
    }

    container.appendChild(renderer.domElement);

    // Add event listener for window resize
    window.addEventListener('resize', onWindowResize);

    animate();
}

function onWindowResize() {
    camera.aspect = container.clientWidth / container.clientHeight;
    camera.updateProjectionMatrix();
    renderer.setSize(container.clientWidth, container.clientHeight);
}

function animate() {
    requestAnimationFrame(animate);
    render();
    rotateCamera();
    moveParticles(); // Call the function to move particles
}

window.ThreeJSFunctions = {
    load: () => { loadScene(); },
};
