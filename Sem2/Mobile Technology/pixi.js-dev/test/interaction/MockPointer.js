'use strict';

/**
 * Use this to mock mouse/touch/pointer events
 *
 * @class
 */
class MockPointer
{
    /**
     * @param {PIXI.Container} stage - The root of the scene tree
     * @param {number} [width=100] - Width of the renderer
     * @param {number} [height=100] - Height of the renderer
     */
    constructor(stage, width, height)
    {
        this.stage = stage;
        this.renderer = new PIXI.CanvasRenderer(width || 100, height || 100);
        this.renderer.sayHello = () => { /* empty */ };
        this.interaction = this.renderer.plugins.interaction;
    }

    /**
     * @private
     * @param {number} x - pointer x position
     * @param {number} y - pointer y position
     */
    setPosition(x, y)
    {
        this.renderer.plugins.interaction.mapPositionToPoint = (point) =>
        {
            point.x = x;
            point.y = y;
        };
    }

    /**
     * @private
     */
    render()
    {
        this.renderer.render(this.stage);
    }

    /**
     * @param {number} x - pointer x position
     * @param {number} y - pointer y position
     */
    mousemove(x, y)
    {
        const mouseEvent = new MouseEvent('mousemove', {
            clientX: x,
            clientY: y,
            preventDefault: sinon.stub(),
        });

        this.setPosition(x, y);
        this.render();
        // mouseOverRenderer state should be correct, so mouse position to view rect
        const rect = new PIXI.Rectangle(0, 0, this.renderer.width, this.renderer.height);

        if (rect.contains(x, y))
        {
            if (!this.interaction.mouseOverRenderer)
            {
                this.interaction.onPointerOver(new MouseEvent('mouseover', {
                    clientX: x,
                    clientY: y,
                    preventDefault: sinon.stub(),
                }));
            }
            this.interaction.onPointerMove(mouseEvent);
        }
        else
        {
            this.interaction.onPointerOut(new MouseEvent('mouseout', {
                clientX: x,
                clientY: y,
                preventDefault: sinon.stub(),
            }));
        }
    }

    /**
     * @param {number} x - pointer x position
     * @param {number} y - pointer y position
     */
    click(x, y)
    {
        this.mousedown(x, y);
        this.mouseup(x, y);
    }

    /**
     * @param {number} x - pointer x position
     * @param {number} y - pointer y position
     */
    mousedown(x, y)
    {
        const mouseEvent = new MouseEvent('mousedown', {
            clientX: x,
            clientY: y,
            preventDefault: sinon.stub(),
        });

        this.setPosition(x, y);
        this.render();
        this.interaction.onPointerDown(mouseEvent);
    }

    /**
     * @param {number} x - pointer x position
     * @param {number} y - pointer y position
     */
    mouseup(x, y)
    {
        const mouseEvent = new MouseEvent('mouseup', {
            clientX: x,
            clientY: y,
            preventDefault: sinon.stub(),
        });

        this.setPosition(x, y);
        this.render();
        this.interaction.onPointerUp(mouseEvent);
    }

    /**
     * @param {number} x - pointer x position
     * @param {number} y - pointer y position
     */
    tap(x, y)
    {
        this.touchstart(x, y);
        this.touchend(x, y);
    }

    /**
     * @param {number} x - pointer x position
     * @param {number} y - pointer y position
     */
    touchstart(x, y)
    {
        const touchEvent = new TouchEvent('touchstart', {
            preventDefault: sinon.stub(),
            changedTouches: [
                new Touch({ identifier: 0, target: this.renderer.view }),
            ],
        });

        this.setPosition(x, y);
        this.render();
        this.interaction.onPointerDown(touchEvent);
    }

    /**
     * @param {number} x - pointer x position
     * @param {number} y - pointer y position
     */
    touchend(x, y)
    {
        const touchEvent = new TouchEvent('touchend', {
            preventDefault: sinon.stub(),
            changedTouches: [
                new Touch({ identifier: 0, target: this.renderer.view }),
            ],
        });

        this.setPosition(x, y);
        this.render();
        this.interaction.onPointerUp(touchEvent);
    }
}

module.exports = MockPointer;
