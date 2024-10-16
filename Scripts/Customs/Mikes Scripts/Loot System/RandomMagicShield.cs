using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class RandomMagicShield : BaseShield
{
    private static string[] prefixes = { "Mighty", "Stalwart", "Mystic", "Enchanted" };
    private static string[] suffixes = { "Aegis", "Bulwark", "Sentinel" };

    private static Random rand = new Random();

    private static Type[] shieldTypes = new Type[]
    {
        typeof(BronzeShield), typeof(Buckler), typeof(HeaterShield), typeof(MetalShield),
        typeof(WoodenShield), typeof(ChaosShield), typeof(OrderShield), typeof(MetalKiteShield)
    };

    private static SkillName[] skillNames = (SkillName[])Enum.GetValues(typeof(SkillName));

    private static Action<BaseShield>[] shieldAttributes = new Action<BaseShield>[]
    {
        (shield) => shield.Attributes.ReflectPhysical = rand.Next(1, 30),
        (shield) => shield.Attributes.DefendChance = rand.Next(1, 30),
        (shield) => shield.Attributes.CastSpeed = rand.Next(0, 2),
        (shield) => shield.Attributes.CastRecovery = rand.Next(0, 3),
        (shield) => shield.Attributes.RegenHits = rand.Next(0, 50),
        (shield) => shield.Attributes.RegenMana = rand.Next(0, 50),
        (shield) => shield.Attributes.RegenStam = rand.Next(0, 50),
        (shield) => shield.Attributes.SpellDamage = rand.Next(0, 50),
        (shield) => shield.Attributes.LowerManaCost = rand.Next(0, 50),
        (shield) => shield.Attributes.LowerRegCost = rand.Next(0, 50),
        (shield) => shield.Attributes.BonusStr = rand.Next(0, 30),
        (shield) => shield.Attributes.BonusDex = rand.Next(0, 30),
        (shield) => shield.Attributes.BonusInt = rand.Next(0, 30),
        (shield) => shield.Attributes.BonusHits = rand.Next(0, 50),
        (shield) => shield.Attributes.BonusStam = rand.Next(0, 50),
        (shield) => shield.Attributes.BonusMana = rand.Next(0, 50),
        (shield) => shield.ArmorAttributes.LowerStatReq = rand.Next(1, 100),
        (shield) => shield.ArmorAttributes.MageArmor = rand.Next(0, 2),
        (shield) => shield.ArmorAttributes.DurabilityBonus = rand.Next(1, 100)
    };

    [Constructable]
    public RandomMagicShield() : base(GetRandomItemID())
    {
        Type selectedType = shieldTypes[rand.Next(shieldTypes.Length)];
        BaseShield tempShield = (BaseShield)Activator.CreateInstance(selectedType);

        string name = prefixes[rand.Next(prefixes.Length)] + " " + suffixes[rand.Next(suffixes.Length)];
        this.Name = name;

        ApplyRandomShieldAttributes(tempShield);
        ApplySkillBonuses();
        
        this.Hue = rand.Next(1, 3001); // Generates a random hue for the shield

        int numberOfSockets = rand.Next(0, 7); // Random sockets
        XmlAttach.AttachTo(this, new XmlSockets(numberOfSockets));

        tempShield.Delete();
    }

    private void ApplyRandomShieldAttributes(BaseShield tempShield)
    {
        List<Action<BaseShield>> selectedAttributes = new List<Action<BaseShield>>(shieldAttributes);
        int numberOfAttributes = rand.Next(5, 11); // Select 5-10 attributes randomly

        for (int i = 0; i < numberOfAttributes; i++)
        {
            int index = rand.Next(selectedAttributes.Count);
            selectedAttributes[index](this);
            selectedAttributes.RemoveAt(index); // Remove to avoid duplicate attributes
        }
    }

    private void ApplySkillBonuses()
    {
        int[] probabilities = new int[] { 10, 20, 30, 40 };
        int totalWeight = 100;
        int randomValue = rand.Next(totalWeight);

        int numberOfSkills = 0;
        int cumulativeWeight = 0;

        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulativeWeight += probabilities[i];
            if (randomValue < cumulativeWeight)
            {
                numberOfSkills = i + 1;
                break;
            }
        }

        for (int i = 0; i < numberOfSkills; i++)
        {
            SkillName skill = skillNames[rand.Next(skillNames.Length)];
            this.SkillBonuses.SetValues(i, skill, rand.Next(1, 21)); // Bonus between 1 and 20
        }
    }

    private static int GetRandomItemID()
    {
        Type selectedType = shieldTypes[rand.Next(shieldTypes.Length)];
        BaseShield tempShield = (BaseShield)Activator.CreateInstance(selectedType);
        int itemID = tempShield.ItemID;
        tempShield.Delete();
        return itemID;
    }

    public RandomMagicShield(Serial serial) : base(serial) { }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}