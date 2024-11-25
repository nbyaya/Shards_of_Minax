using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Custom
{
    public class SilverSlimeBoss : SilverSlime
    {
        [Constructable]
        public SilverSlimeBoss() : base()
        {
            Name = "The Silver Slime Overlord";
            Body = 51; // Retaining the original body
            BaseSoundID = 456;
            Team = Utility.RandomMinMax(1, 5);

            // Enhanced stats (matching or exceeding Barracoon's)
            SetStr(425); // Stronger strength
            SetDex(150); // Higher dexterity
            SetInt(750); // Higher intelligence

            SetHits(12000); // Boss health
            SetDamage(30, 45); // Increased damage range

            // Improved resistances
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 40, 60);

            // Improved skills
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 22500; // High fame for a boss
            Karma = -22500; // High negative karma for a boss

            VirtualArmor = 70;

            Tamable = false; // Boss cannot be tamed
            ControlSlots = 0; // No control slots

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);
            
            if (from != null && !willKill && amount > 0 && from != this)
            {
                // Absorb spells cast on the SilverSlimeBoss and cast Harm back
                if (from is BaseCreature && ((BaseCreature)from).Summoned)
                {
                    from.Damage(amount, this);
                    this.Hits += amount;
                    this.Mana += amount;
                    this.Stam += amount;
                    this.Say("Slurp!");
                    this.DoHarm(from);
                }
                else if (from is Mobile && from.Spell != null)
                {
                    from.Damage(amount, this);
                    this.Hits += amount;
                    this.Mana += amount;
                    this.Stam += amount;
                    this.Say("Slurp!");
                    this.DoHarm(from);
                }
            }
        }

        public void DoHarm(Mobile target)
        {
            // Cast Harm spell on the target
            if (target != null && target.Alive && this.CanSee(target) && this.InLOS(target))
            {
                this.Say("Harm!");
                this.MovingParticles(target, 0x36E4, 5, 0, false, true, 3006, 4006, 0);
                target.PlaySound(0x1F1);
                SpellHelper.Damage(TimeSpan.Zero, target, this, Utility.RandomMinMax(12, 20));
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (attacker != null && attacker.Alive && 0.1 > Utility.RandomDouble())
            {
                // Spawn a new SilverSlime when attacked
                this.Say("Split!");
                SilverSlime spawn = new SilverSlime();
                spawn.MoveToWorld(this.Location, this.Map);
                spawn.Combatant = attacker;
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public SilverSlimeBoss(Serial serial) : base(serial)
        {
        }

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
}
