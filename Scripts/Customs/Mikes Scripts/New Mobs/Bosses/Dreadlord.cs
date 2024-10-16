using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a dreadlord's corpse")]
    public class Dreadlord : BaseCreature
    {
        public override bool AutoDispel { get { return true; } } // Dispel summoned creatures

        [Constructable]
        public Dreadlord()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Dreadlord";
            Body = 0x190; // Human male body
            Hue = 0x497; // Dark red hue
            BaseSoundID = 0x165;

            SetStr(1000, 1200);
            SetDex(150, 200);
            SetInt(1000, 1200);

            SetHits(5000, 6000);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 150.0, 170.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 60;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && 0.2 > Utility.RandomDouble()) // 20% chance to use special attack
            {
                SpecialAttack(Combatant as Mobile);
            }
        }

        public void SpecialAttack(Mobile target)
        {
            if (target == null)
                return;

            switch (Utility.Random(5)) // Randomly choose one of the five special attacks
            {
                case 0:
                    PoisonAttack(target);
                    break;
                case 1:
                    CurseAttack(target);
                    break;
                case 2:
                    ParalyzeAttack(target);
                    break;
                case 3:
                    DrainLifeAttack(target);
                    break;
                case 4:
                    FireBreathAttack();
                    break;
            }
        }

        public void PoisonAttack(Mobile target)
        {
            target.ApplyPoison(this, Poison.Deadly);
            target.SendMessage("You feel a deadly poison coursing through your veins!");
        }

        public void CurseAttack(Mobile target)
        {
            target.SendMessage("You have been cursed by the Dreadlord!");
            target.RawStr -= 10; // Reduce Strength
            target.RawDex -= 10; // Reduce Dexterity
            target.RawInt -= 10; // Reduce Intelligence

            Timer.DelayCall(TimeSpan.FromSeconds(30), delegate // Curse lasts for 30 seconds
            {
                target.RawStr += 10;
                target.RawDex += 10;
                target.RawInt += 10;
                target.SendMessage("The curse has worn off.");
            });
        }

        public void ParalyzeAttack(Mobile target)
        {
            if (target.Paralyzed || target.Frozen)
                return;

            target.Paralyze(TimeSpan.FromSeconds(5.0)); // Paralyze target for 5 seconds
            target.SendMessage("You are paralyzed by the Dreadlord!");
        }

        public void DrainLifeAttack(Mobile target)
        {
            int damage = Utility.RandomMinMax(30, 50);
            target.Damage(damage, this);
            this.Hits += damage; // Heal the Dreadlord by the damage dealt
            target.SendMessage("The Dreadlord drains your life!");
        }

        public void FireBreathAttack()
        {
            this.Say("Feel the flames of doom!");

            Map map = this.Map;

            if (map == null)
                return;

            foreach (Mobile m in this.GetMobilesInRange(8)) // AoE attack within 8 tile radius
            {
                if (m == this || !this.CanBeHarmful(m))
                    continue;

                if (this.CanBeHarmful(m, false))
                {
                    this.DoHarmful(m);
                    m.Damage(Utility.RandomMinMax(30, 60), this); // Fire damage
                    m.SendMessage("You are scorched by the Dreadlord's flames!");
                }
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Rich);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            c.DropItem(new Gold(2000, 3000));
            c.DropItem(new DreadlordSword()); // Example of a unique item drop
        }

        public Dreadlord(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    // Example of a unique item drop: Dreadlord's Sword
    public class DreadlordSword : Longsword
    {
        [Constructable]
        public DreadlordSword()
        {
            Name = "Dreadlord's Sword";
            Hue = 0x497;
            Attributes.WeaponDamage = 50;
            Attributes.WeaponSpeed = 20;
            Attributes.Luck = 100;
            Slayer = SlayerName.DaemonDismissal;
        }

        public DreadlordSword(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
