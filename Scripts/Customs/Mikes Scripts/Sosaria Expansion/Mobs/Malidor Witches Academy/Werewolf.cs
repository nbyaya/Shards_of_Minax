using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a werewolf corpse")]
    public class Werewolf : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextManaHowlTime;
        private DateTime m_NextArcaneBurstTime;
        private DateTime m_NextWolfSummonTime;

        // Unique magic-themed hue (choose a value that fits your aesthetic)
        private const int UniqueHue = 1348;

        // Timer for transformation/disguise
        private Timer m_DisguiseTimer;

        [Constructable]
        public Werewolf()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a werewolf";
            Body = 246; // Using the same body as the BakeKitsune.
            BaseSoundID = 0x4DE; // Same base sound ID.
            Hue = UniqueHue;

            // --- Advanced Stats ---
            SetStr(400, 500);
            SetDex(250, 300);
            SetInt(500, 600);

            SetHits(1200, 1600);
            SetStam(250, 300);
            SetMana(500, 600);

            SetDamage(20, 28);
            // Split damage evenly between physical and energy (magic-themed)
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 70, 80);

            // --- Enhanced Skills ---
            SetSkill(SkillName.EvalInt, 110.1, 120.0);
            SetSkill(SkillName.Magery, 110.1, 125.0);
            SetSkill(SkillName.MagicResist, 120.2, 130.0);
            SetSkill(SkillName.Tactics, 110.1, 120.0);
            SetSkill(SkillName.Wrestling, 110.1, 120.0);
            SetSkill(SkillName.Anatomy, 90.0, 100.0);

            Fame = 20000;
            Karma = -20000;
            VirtualArmor = 80;
            ControlSlots = 5;

            // --- Initialize Ability Cooldowns ---
            m_NextManaHowlTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextArcaneBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextWolfSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            // --- Loot ---
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 15)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(3, 8)));

            // Optional very-rare drop (assumes LunarShard is defined elsewhere)
            if (Utility.RandomDouble() < 0.005)
                PackItem(new MaxxiaScroll());
        }

        // --- Unique Ability: Mana Howl --- 
        // Drains mana from the nearby target and heals the Werewolf for a portion of what is drained.
        public void ManaHowlAttack()
        {
            if (Combatant == null || Map == null)
                return;

            if (!(Combatant is Mobile target))
                return;

            if (!this.InRange(target.Location, 8))
                return;

            PlaySound(0x4DF); // Use an appropriate howl sound
            FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot); // Magical particle effect

            int drain = Utility.RandomMinMax(10, 20);
            if (target.Mana >= drain)
            {
                target.Mana -= drain;
                this.Hits += drain / 2; // Heal half the drained mana as hit points
                target.SendMessage("Your magical energy is drained by the werewolf's howl!");
            }
            else if (target.Mana > 0)
            {
                drain = target.Mana;
                target.Mana = 0;
                this.Hits += drain / 2;
                target.SendMessage("Your magical energy is drained by the werewolf's howl!");
            }
        }

        // --- Unique Ability: Arcane Burst ---
        // Deals a burst of pure energy damage to all valid targets in a 6-tile radius.
        public void ArcaneBurstAttack()
        {
            if (Map == null)
                return;

            PlaySound(0x4DE);
            FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet); // Explosion-style magical effect

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5029, 0);
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(35, 50);
                    // Apply 100% energy damage: parameters are (phys, fire, cold, poison, energy)
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
                }
            }
        }

        // --- Unique Ability: Summon Spectral Wolves ---
        // Summons 2-4 spectral wolf allies to harass the current combatant.
        public void SummonSpectralWolves()
        {
            if (Map == null)
                return;

            int count = Utility.RandomMinMax(2, 4);
            for (int i = 0; i < count; i++)
            {
                int xOff = Utility.RandomMinMax(-2, 2);
                int yOff = Utility.RandomMinMax(-2, 2);
                Point3D spawnLoc = new Point3D(this.X + xOff, this.Y + yOff, this.Z);
                if (Map.CanFit(spawnLoc, 16))
                {
                    // Assumes SpectralWolf is defined elsewhere in your shard.
                    SpectralWolf wolf = new SpectralWolf();
                    wolf.Team = this.Team;
                    wolf.MoveToWorld(spawnLoc, Map);
                    if (Combatant is Mobile target)
                        wolf.Combatant = target;
                }
            }
        }

        // --- OnThink Override ---
        // Checks ability cooldowns and also the transformation logic.
        public override void OnThink()
        {
            base.OnThink();

            // If there is no current combatant, allow transformation into human form
            if (Combatant == null || Map == null || Map == Map.Internal)
            {
                if (!IsBodyMod && !Controlled && m_DisguiseTimer == null && Utility.RandomBool())
                {
                    m_DisguiseTimer = Timer.DelayCall(TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30)), new TimerCallback(Disguise));
                }
                return;
            }

            // Ability: Mana Howl Attack
            if (DateTime.UtcNow >= m_NextManaHowlTime && this.InRange(Combatant.Location, 8))
            {
                ManaHowlAttack();
                m_NextManaHowlTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }

            // Ability: Arcane Burst Attack
            if (DateTime.UtcNow >= m_NextArcaneBurstTime && this.InRange(Combatant.Location, 6))
            {
                ArcaneBurstAttack();
                m_NextArcaneBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }

            // Ability: Summon Spectral Wolves
            if (DateTime.UtcNow >= m_NextWolfSummonTime && this.InRange(Combatant.Location, 12))
            {
                SummonSpectralWolves();
                m_NextWolfSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // --- Transformation into Human Form (Disguise) ---
        // Mimics the BakeKitsune transformation but with a werewolf twist.
        public void Disguise()
        {
            if (Combatant != null || IsBodyMod || Controlled)
                return;

            FixedEffect(0x376A, 8, 32);
            PlaySound(0x1FE);

            Female = Utility.RandomBool();
            if (Female)
            {
                BodyMod = 0x191;
                Name = NameList.RandomName("female");
            }
            else
            {
                BodyMod = 0x190;
                Name = NameList.RandomName("male");
            }
            Title = "the unsuspecting mage";
            Hue = Race.Human.RandomSkinHue();
            HairItemID = Race.Human.RandomHair(this);
            HairHue = Race.Human.RandomHairHue();
            FacialHairItemID = Race.Human.RandomFacialHair(this);
            FacialHairHue = HairHue;

            // Equip simple clothing appropriate for a mage in disguise
            switch (Utility.Random(4))
            {
                case 0:
                    AddItem(new Shoes(Utility.RandomNeutralHue()));
                    break;
                case 1:
                    AddItem(new Boots(Utility.RandomNeutralHue()));
                    break;
                case 2:
                    AddItem(new Sandals(Utility.RandomNeutralHue()));
                    break;
                case 3:
                    AddItem(new ThighBoots(Utility.RandomNeutralHue()));
                    break;
            }
            AddItem(new Robe(Utility.RandomNondyedHue()));

            m_DisguiseTimer = Timer.DelayCall(TimeSpan.FromSeconds(75), new TimerCallback(RemoveDisguise));
        }

        // Revert back to werewolf form
        public void RemoveDisguise()
        {
            if (!IsBodyMod)
                return;

            Name = "a werewolf";
            Title = null;
            BodyMod = 0;
            Hue = UniqueHue;
            HairItemID = 0;
            HairHue = 0;
            FacialHairItemID = 0;
            FacialHairHue = 0;

            DeleteItemOnLayer(Layer.OuterTorso);
            DeleteItemOnLayer(Layer.Shoes);

            m_DisguiseTimer = null;
        }

        public void DeleteItemOnLayer(Layer layer)
        {
            Item item = FindItemOnLayer(layer);
            if (item != null)
                item.Delete();
        }

        public override void OnCombatantChange()
        {
            if (Combatant == null && !IsBodyMod && !Controlled && m_DisguiseTimer == null && Utility.RandomBool())
                m_DisguiseTimer = Timer.DelayCall(TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30)), new TimerCallback(Disguise));
        }

        public override bool OnBeforeDeath()
        {
            RemoveDisguise();
            return base.OnBeforeDeath();
        }

        // --- Standard Sound Overrides ---
        public override int GetAngerSound() { return 0x4DE; }
        public override int GetIdleSound() { return 0x4DD; }
        public override int GetAttackSound() { return 0x4DC; }
        public override int GetHurtSound() { return 0x4DF; }
        public override int GetDeathSound() { return 0x4DB; }

        public Werewolf(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reinitialize ability cooldowns upon load
            m_NextManaHowlTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextArcaneBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextWolfSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            Timer.DelayCall(TimeSpan.Zero, new TimerCallback(RemoveDisguise));
        }
    }
}
