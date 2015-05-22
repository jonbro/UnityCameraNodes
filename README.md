# Unity Tool: Camera Nodes

A basic tool for creating myst style camera node navigation. Check the example file for a simple scene that uses it.

For those that like watching videos, here is a quick demo: https://www.youtube.com/watch?v=XQDDzJ-wMoE (3mins)

# HOWTO

1. Create a scene with a basic camera in it.
2. Attach the *Node Camera* script to the camera.
3. Create an empty game object, and attach the *Camera Node* script to it. This is also accessible through the Game Object / Camera Nodes / New Camera Node menu item.
5. Set the Camera Root Node on the *Node Camera* component to the first node in the scene.
6. While the node object is selected, you can click "Add Node" in the toolbar to add a new node that is walkable from this node.
7. In the inspector, the list of attached nodes is displayed. Pressing left moves the view to point at the node up the list, and pressing right moves down the list. Pressing up moves to the node that you are looking at if it is not set to walkable. 

![Imgur](http://i.imgur.com/1qYKpQ2.png)

# ADDITIONAL FEATURES

You can create node behaviours to attach to nodes that operate when they are either looked at, or entered. There are examples of two of these in the scripts/NodeBehaviours directory.
