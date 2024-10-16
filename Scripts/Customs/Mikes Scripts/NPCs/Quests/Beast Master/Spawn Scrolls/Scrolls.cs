using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{
    public class RandomAngelScroll : SpawnScroll
    {
        [Constructable]
        public RandomAngelScroll() : base(typeof(RandomAngel))
        {
        }

        public RandomAngelScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomAnimatedPlantScroll : SpawnScroll
    {
        [Constructable]
        public RandomAnimatedPlantScroll() : base(typeof(RandomAnimatedPlant))
        {
        }

        public RandomAnimatedPlantScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomApeScroll : SpawnScroll
    {
        [Constructable]
        public RandomApeScroll() : base(typeof(RandomApe))
        {
        }

        public RandomApeScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomArachnidScroll : SpawnScroll
    {
        [Constructable]
        public RandomArachnidScroll() : base(typeof(RandomArachnid))
        {
        }

        public RandomArachnidScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomBearScroll : SpawnScroll
    {
        [Constructable]
        public RandomBearScroll() : base(typeof(RandomBear))
        {
        }

        public RandomBearScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomBeholderScroll : SpawnScroll
    {
        [Constructable]
        public RandomBeholderScroll() : base(typeof(RandomBeholder))
        {
        }

        public RandomBeholderScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomBovineScroll : SpawnScroll
    {
        [Constructable]
        public RandomBovineScroll() : base(typeof(RandomBovine))
        {
        }

        public RandomBovineScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomCanineScroll : SpawnScroll
    {
        [Constructable]
        public RandomCanineScroll() : base(typeof(RandomCanine))
        {
        }

        public RandomCanineScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomCatScroll : SpawnScroll
    {
        [Constructable]
        public RandomCatScroll() : base(typeof(RandomCat))
        {
        }

        public RandomCatScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomDemonScroll : SpawnScroll
    {
        [Constructable]
        public RandomDemonScroll() : base(typeof(RandomDemon))
        {
        }

        public RandomDemonScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomDragonScroll : SpawnScroll
    {
        [Constructable]
        public RandomDragonScroll() : base(typeof(RandomDragon))
        {
        }

        public RandomDragonScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomElementalScroll : SpawnScroll
    {
        [Constructable]
        public RandomElementalScroll() : base(typeof(RandomElemental))
        {
        }

        public RandomElementalScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomFeyScroll : SpawnScroll
    {
        [Constructable]
        public RandomFeyScroll() : base(typeof(RandomFey))
        {
        }

        public RandomFeyScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomGargoyleScroll : SpawnScroll
    {
        [Constructable]
        public RandomGargoyleScroll() : base(typeof(RandomGargoyle))
        {
        }

        public RandomGargoyleScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomGiantScroll : SpawnScroll
    {
        [Constructable]
        public RandomGiantScroll() : base(typeof(RandomGiant))
        {
        }

        public RandomGiantScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomInsectScroll : SpawnScroll
    {
        [Constructable]
        public RandomInsectScroll() : base(typeof(RandomInsect))
        {
        }

        public RandomInsectScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomKnightLordScroll : SpawnScroll
    {
        [Constructable]
        public RandomKnightLordScroll() : base(typeof(RandomKnightLord))
        {
        }

        public RandomKnightLordScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomLizardScroll : SpawnScroll
    {
        [Constructable]
        public RandomLizardScroll() : base(typeof(RandomLizard))
        {
        }

        public RandomLizardScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomMinotaurScroll : SpawnScroll
    {
        [Constructable]
        public RandomMinotaurScroll() : base(typeof(RandomMinotaur))
        {
        }

        public RandomMinotaurScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomMonsterScroll : SpawnScroll
    {
        [Constructable]
        public RandomMonsterScroll() : base(typeof(RandomMonster))
        {
        }

        public RandomMonsterScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomOrcScroll : SpawnScroll
    {
        [Constructable]
        public RandomOrcScroll() : base(typeof(RandomOrc))
        {
        }

        public RandomOrcScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomPigScroll : SpawnScroll
    {
        [Constructable]
        public RandomPigScroll() : base(typeof(RandomPig))
        {
        }

        public RandomPigScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomRidableScroll : SpawnScroll
    {
        [Constructable]
        public RandomRidableScroll() : base(typeof(RandomRidable))
        {
        }

        public RandomRidableScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomRobotScroll : SpawnScroll
    {
        [Constructable]
        public RandomRobotScroll() : base(typeof(RandomRobot))
        {
        }

        public RandomRobotScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomRodentScroll : SpawnScroll
    {
        [Constructable]
        public RandomRodentScroll() : base(typeof(RandomRodent))
        {
        }

        public RandomRodentScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomRuminantScroll : SpawnScroll
    {
        [Constructable]
        public RandomRuminantScroll() : base(typeof(RandomRuminant))
        {
        }

        public RandomRuminantScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomSlimeScroll : SpawnScroll
    {
        [Constructable]
        public RandomSlimeScroll() : base(typeof(RandomSlime))
        {
        }

        public RandomSlimeScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomSnakeScroll : SpawnScroll
    {
        [Constructable]
        public RandomSnakeScroll() : base(typeof(RandomSnake))
        {
        }

        public RandomSnakeScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomUndeadScroll : SpawnScroll
    {
        [Constructable]
        public RandomUndeadScroll() : base(typeof(RandomUndead))
        {
        }

        public RandomUndeadScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    public class RandomWizardWitchScroll : SpawnScroll
    {
        [Constructable]
        public RandomWizardWitchScroll() : base(typeof(RandomWizardWitch))
        {
        }

        public RandomWizardWitchScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }
}
